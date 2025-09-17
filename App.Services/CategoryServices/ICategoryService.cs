using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<ServiceResult<CreateCategoryResponse>> CreateAsync(CreateCategoryRequest request);
        Task<ServiceResult> UpdateAsync(int id, UpdateCategoryRequest request);
        Task<ServiceResult<List<CategoryDto>>> GetAllList();
        Task<ServiceResult> DeleteAsync(int id);

    }
}
