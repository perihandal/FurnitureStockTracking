using App.Repositories;
using App.Repositories.Categories;
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
                p.Code,
                p.Name,
                p.Company.Name,
                p.Branch.Name,
                p.User.FullName,
                p.CreateDate
            )).ToList();


            return ServiceResult<List<CategoryDto>>.Success(stockcardAsDto);
        }


    }
}
