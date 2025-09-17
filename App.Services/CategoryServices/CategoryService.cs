using App.Repositories;
using App.Repositories.Categories;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace App.Services.CategoryServices
{
    public class CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : ICategoryService
    {
        public async Task<ServiceResult<CreateCategoryResponse>> CreateAsync(CreateCategoryRequest request)
        {
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
            var category = await categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
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

            var stockcardAsDto = categories.Select(p => new CategoryDto(
                p.Id,
                p.Code,
                p.IsActive,
                p.Name,
                p.Company.Id,
                p.Company.Name,
                p.Branch.Id,
                p.Branch.Name,
                p.User.FullName,
                p.CreateDate
            )).ToList();


            return ServiceResult<List<CategoryDto>>.Success(stockcardAsDto);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
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
