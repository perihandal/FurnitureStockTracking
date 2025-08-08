using App.Services.CategoryServices;
using App.Services.StockCardServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    public class CategoryController(ICategoryService categoryService) : CustomBaseController
    {
        [HttpPost]//--->eklme yaparken
        public async Task<IActionResult> Create(CreateCategoryRequest request) => CreateActionResult(await categoryService.CreateAsync(request));

        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateCategoryRequest request) => CreateActionResult(await categoryService.UpdateAsync(id, request));

        [HttpGet] //---> istek yaparken
        public async Task<IActionResult> GetAll() => CreateActionResult(await categoryService.GetAllList());
    }
}
