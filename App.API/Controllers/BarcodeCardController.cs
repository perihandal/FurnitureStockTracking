using App.Repositories.BarcodeCards;
using App.Services.BarcodeCardServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.API.Auth;

namespace App.API.Controllers
{
    [Authorize]
    [CompanyAuthorize] // Şirket bazlı filtreleme
    public class BarcodeCardController(IBarcodeCardService barcodeCardService) : CustomBaseController
    {
        [HttpGet] // Tüm roller okuyabilir (kendi şirketinde)
        public async Task<IActionResult> GetAll() => CreateActionResult(await barcodeCardService.GetAllListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => CreateActionResult(await barcodeCardService.GetByIdAsync(id));

        [HttpGet("by-stockcard/{stockCardId}")]
        public async Task<IActionResult> GetByStockCardId(int stockCardId) => CreateActionResult(await barcodeCardService.GetByStockCardIdAsync(stockCardId));

        [RoleAuthorize("Admin", "Editor")] // Sadece Admin ve Editor oluşturabilir
        [HttpPost]
        public async Task<IActionResult> Create(CreateBarcodeCardRequest request) => CreateActionResult(await barcodeCardService.CreateAsync(request));

        [RoleAuthorize("Admin", "Editor")] // Sadece Admin ve Editor güncelleyebilir
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateBarcodeCardRequest request) => CreateActionResult(await barcodeCardService.UpdateAsync(id, request));

        [RoleAuthorize("Admin", "Editor")] // Sadece Admin ve Editor silebilir
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => CreateActionResult(await barcodeCardService.DeleteAsync(id));

        [RoleAuthorize("Admin", "Editor")] // Sadece Admin ve Editor default ayarlayabilir
        [HttpPut("{id}/set-default")]
        public async Task<IActionResult> SetAsDefault(int id) => CreateActionResult(await barcodeCardService.SetAsDefaultAsync(id));

        [HttpGet("validate")] // Tüm roller barcode okuyabilir
        public async Task<IActionResult> ValidateBarcode([FromQuery] string barcodeCode, [FromQuery] BarcodeType barcodeType) => CreateActionResult(await barcodeCardService.ValidateBarcodeAsync(barcodeCode, barcodeType));
    }
}