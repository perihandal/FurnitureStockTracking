using App.Services.CategoryServices;
using App.Services.WareHouseServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.API.Auth;

namespace App.API.Controllers
{
    [Authorize]
    [WarehouseAuthorize] // Depo bazlı yetkilendirme
    public class WarehouseController(IWareHouseService wareHouseService) : CustomBaseController
    {
        [RoleAuthorize("Admin", "Editor")] // Sadece Admin ve Editor oluşturabilir
        [HttpPost]
        public async Task<IActionResult> Create(CreateWareHouseRequest request) => CreateActionResult(await wareHouseService.CreateAsync(request));

        [RoleAuthorize("Admin", "Editor")] // Sadece Admin ve Editor güncelleyebilir
        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateWareHouseRequest request) => CreateActionResult(await wareHouseService.UpdateAsync(id, request));

        [HttpGet] // Tüm roller okuyabilir (yetki seviyesine göre filtrelenmiş)
        public async Task<IActionResult> GetAll() => CreateActionResult(await wareHouseService.GetAllList());

        [HttpGet("for-transfer")] // Transfer için kullanılabilir depoları getir
        public async Task<IActionResult> GetForTransfer() => CreateActionResult(await wareHouseService.GetAllList());

        [RoleAuthorize("Admin")] // Sadece Admin silebilir
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => CreateActionResult(await wareHouseService.DeleteAsync(id));
    }
}
