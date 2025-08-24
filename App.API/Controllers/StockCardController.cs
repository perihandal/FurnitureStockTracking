using App.Services.StockCardServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    public class StockCardController(IStockCardService stockcardService) : CustomBaseController
    {

        [HttpGet] //---> istek yaparken
        public async Task<IActionResult> GetAll() => CreateActionResult(await stockcardService.GetAllList());
        [HttpGet("{count:int}")]
        //public async Task<IActionResult> GetTopPriceASync() => CreateActionResult(await stockcardService.GetAllList());
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int id) => CreateActionResult(await productService.GetByIdAsync(id));
        [HttpPost]//--->eklme yaparken
        public async Task<IActionResult> Create(CreateStockCardRequest request) => CreateActionResult(await stockcardService.CreateAsync(request));
        [HttpPut("{id:int}")]//---> güncelleme yaparken
        public async Task<IActionResult> Update(int id, UpdateStockCardRequest request) => CreateActionResult(await stockcardService.UpdateAsync(id, request));

        [HttpGet("{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetPagedAll (int pageNumber, int pageSize) =>
        CreateActionResult(await stockcardService.GetPagedAllListAsync(pageNumber, pageSize));


    }
}
