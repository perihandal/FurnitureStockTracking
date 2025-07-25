using App.Services.ProductServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API
{
    public class ProductsController(IProductService productService) : CustomBaseController
    {

        [HttpGet] //---> istek yaparken
        public async Task<IActionResult> GetAll() => CreateActionResult(await productService.GetAllList());
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => CreateActionResult(await productService.GetByIdAsync(id));
        [HttpPost]//--->eklme yaparken
        public async Task<IActionResult> Create(CreateProductRequest request) => CreateActionResult(await productService.CreateAsync(request));
        [HttpPut("{id}")]//---> güncelleme yaparken
        public async Task<IActionResult> Update(int id,UpdateProductRequest request) => CreateActionResult(await productService.UpdateAsync(id, request));
         

    }
}
