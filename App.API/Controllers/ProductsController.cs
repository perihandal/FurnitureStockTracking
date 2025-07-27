using App.Services.StockCardServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    public class ProductsController(IStockCardService stockcardService) : CustomBaseController
    {

        //[HttpGet] //---> istek yaparken
        //public async Task<IActionResult> GetAll() => CreateActionResult(await productService.GetAllList());
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int id) => CreateActionResult(await productService.GetByIdAsync(id));
        //[HttpPost]//--->eklme yaparken
        //public async Task<IActionResult> Create(CreateStockCardRequest request) => CreateActionResult(await productService.CreateAsync(request));
        //[HttpPut("{id}")]//---> güncelleme yaparken
        //public async Task<IActionResult> Update(int id, UpdateStockCardRequest request) => CreateActionResult(await productService.UpdateAsync(id, request));


    }
}
