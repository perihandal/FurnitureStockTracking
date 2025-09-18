using App.Repositories.BarcodeCards;
using App.Services.BarcodeCardServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [Authorize]

    public class BarcodeCardController(IBarcodeCardService barcodeCardService) : CustomBaseController
    {
        [HttpGet] //---> istek yaparken
        public async Task<IActionResult> GetAll() => CreateActionResult(await barcodeCardService.GetAllListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => CreateActionResult(await barcodeCardService.GetByIdAsync(id));

        [HttpGet("by-stockcard/{stockCardId}")]
        public async Task<IActionResult> GetByStockCardId(int stockCardId) => CreateActionResult(await barcodeCardService.GetByStockCardIdAsync(stockCardId));

        [Authorize(Roles = "Admin,Editor")]
        [HttpPost]//--->eklme yaparken
        public async Task<IActionResult> Create(CreateBarcodeCardRequest request) => CreateActionResult(await barcodeCardService.CreateAsync(request));

        [Authorize(Roles = "Admin,Editor")]
        [HttpPut("{id}")]//---> güncelleme yaparken
        public async Task<IActionResult> Update(int id, UpdateBarcodeCardRequest request) => CreateActionResult(await barcodeCardService.UpdateAsync(id, request));

        [Authorize(Roles = "Admin,Editor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => CreateActionResult(await barcodeCardService.DeleteAsync(id));

        [Authorize(Roles = "Admin,Editor")]
        [HttpPut("{id}/set-default")]
        public async Task<IActionResult> SetAsDefault(int id) => CreateActionResult(await barcodeCardService.SetAsDefaultAsync(id));

        [HttpGet("validate")]
        public async Task<IActionResult> ValidateBarcode([FromQuery] string barcodeCode, [FromQuery] BarcodeType barcodeType) => CreateActionResult(await barcodeCardService.ValidateBarcodeAsync(barcodeCode, barcodeType));
    }
}