using App.Repositories;
using App.Repositories.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace App.Services.CategoryServices
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IUnitOfWork unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) 
            : base(httpContextAccessor)
        {
            this.categoryRepository = categoryRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<ServiceResult<CreateCategoryResponse>> CreateAsync(CreateCategoryRequest request)
        {
            // User rolü create işlemi yapamaz
            if (IsUser())
            {
                return ServiceResult<CreateCategoryResponse>.Fail("Kategori oluşturma yetkiniz bulunmamaktadır.", HttpStatusCode.Forbidden);
            }

            // CompanyId ve BranchId doğrulaması
            var accessValidation = ValidateEntityAccess(request.CompanyId, request.BranchId);
            if (!accessValidation.IsSuccess)
            {
                return ServiceResult<CreateCategoryResponse>.Fail(accessValidation.ErrorMessage!, accessValidation.Status);
            }

            var category = new Category()
            {
                Name = request.Name,
                Code = request.Code,
                CompanyId = request.CompanyId,
                BranchId = request.BranchId,
                UserId = request.UserId,
                 
                // default değerler:
                IsActive = true,
                CreateDate = DateTime.UtcNow
            };

            await categoryRepository.AddAsync(category);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult<CreateCategoryResponse>.Success(new CreateCategoryResponse(category.Id));
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateCategoryRequest request)
        {
            // User rolü update işlemi yapamaz
            if (IsUser())
            {
                return ServiceResult.Fail("Kategori güncelleme yetkiniz bulunmamaktadır.", HttpStatusCode.Forbidden);
            }

            var category = await categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return ServiceResult.Fail("Category not found", HttpStatusCode.NotFound);
            }

            // Mevcut entity'ye erişim kontrolü
            if (!CanAccessEntity(category.CompanyId, category.BranchId))
            {
                return ServiceResult.Fail("Bu kategoriye erişim yetkiniz bulunmamaktadır.", HttpStatusCode.Forbidden);
            }

            // Yeni veriler için erişim kontrolü
            var accessValidation = ValidateEntityAccess(request.CompanyId, request.BranchId);
            if (!accessValidation.IsSuccess)
            {
                return ServiceResult.Fail(accessValidation.ErrorMessage!, accessValidation.Status);
            }

            category.Name = request.Name;
            category.Code = request.Code;
            category.CompanyId = request.CompanyId;
            category.BranchId = request.BranchId;
            category.UserId = request.UserId;
            category.IsActive = request.IsActive;
            category.CreateDate = DateTime.UtcNow;

            categoryRepository.Update(category);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<List<CategoryDto>>> GetAllList()
        {
            var categories = await categoryRepository.GetAllWithDetailsAsync();

            // Admin değilse, sadece kendi company/branch verilerine erişebilir
            if (!IsAdmin())
            {
                var userCompanyId = GetUserCompanyId();
                var userBranchId = GetUserBranchId();

                if (userCompanyId.HasValue)
                {
                    categories = categories.Where(c => c.CompanyId == userCompanyId.Value).ToList();
                }

                // User rolü için branch kontrolü
                if (IsUser() && userBranchId.HasValue)
                {
                    categories = categories.Where(c => c.BranchId == userBranchId.Value).ToList();
                }
            }

            var stockcardAsDto = categories.Select(p => new CategoryDto(
                p.Id,
                p.Code,
                p.IsActive,
                p.Name,
                p.Company.Id,
                p.Company.Name,
                p.Branch.Id,
                p.Branch.Name,
                p.User?.FullName ?? "",
                p.CreateDate
            )).ToList();

            return ServiceResult<List<CategoryDto>>.Success(stockcardAsDto);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            // User rolü delete işlemi yapamaz
            if (IsUser())
            {
                return ServiceResult.Fail("Kategori silme yetkiniz bulunmamaktadır.", HttpStatusCode.Forbidden);
            }

            // Category'i navigation property'leri ile birlikte al
            var category = await categoryRepository.Where(c => c.Id == id)
                .Include(c => c.StockCards)
                    .ThenInclude(sc => sc.BarcodeCards)
                .Include(c => c.StockCards)
                    .ThenInclude(sc => sc.PriceDefinitions)
                        .ThenInclude(pd => pd.PriceHistories)
                .FirstOrDefaultAsync();

            if (category == null)
                return ServiceResult.Fail("Category not found", HttpStatusCode.NotFound);

            // Entity'ye erişim kontrolü
            if (!CanAccessEntity(category.CompanyId, category.BranchId))
            {
                return ServiceResult.Fail("Bu kategoriye erişim yetkiniz bulunmamaktadır.", HttpStatusCode.Forbidden);
            }

            // Category soft delete
            category.IsActive = false;

            // Category'e ait StockCards soft delete
            if (category.StockCards != null && category.StockCards.Any())
            {
                foreach (var sc in category.StockCards)
                {
                    sc.IsActive = false;

                    // StockCard -> BarcodeCards
                    if (sc.BarcodeCards != null && sc.BarcodeCards.Any())
                    {
                        foreach (var bc in sc.BarcodeCards)
                            bc.IsActive = false;
                    }

                    // StockCard -> PriceDefinitions ve PriceHistories
                    if (sc.PriceDefinitions != null && sc.PriceDefinitions.Any())
                    {
                        foreach (var pd in sc.PriceDefinitions)
                        {
                            pd.IsActive = false;

                            if (pd.PriceHistories != null && pd.PriceHistories.Any())
                            {
                                foreach (var ph in pd.PriceHistories)
                                    ph.IsActive = false;
                            }
                        }
                    }
                }
            }

            // Update ve kaydet
            categoryRepository.Update(category);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }



    }
}
