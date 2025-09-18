using App.API.Auth;
using App.Services.CategoryServices;
using App.Services.StockCardServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [CompanyAuthorize]
    [Authorize(Roles = "Admin,Editor,User")]
    public class CategoryController(ICategoryService categoryService) : CustomBaseController
    {
        [HttpPost]//--->eklme yaparken
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Create(CreateCategoryRequest request) => CreateActionResult(await categoryService.CreateAsync(request));

        [HttpPut]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Update(int id, UpdateCategoryRequest request) => CreateActionResult(await categoryService.UpdateAsync(id, request));

        [HttpGet] //---> istek yaparken
        public async Task<IActionResult> GetAll() => CreateActionResult(await categoryService.GetAllList());

        [HttpDelete]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await categoryService.DeleteAsync(id);
            return CreateActionResult(result);
        }
    }
}
