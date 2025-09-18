using App.Services.StockCardServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using App.API.Auth;

namespace App.API.Controllers
{
    [Authorize(Roles = "Admin,Editor,User")]
    [CompanyAuthorize] // Tüm işlemler şirket bazlı
    public class StockCardController(IStockCardService stockcardService) : CustomBaseController
    {
        [HttpGet] // Tüm roller okuyabilir (kendi şirketinde)
        public async Task<IActionResult> GetAll() => CreateActionResult(await stockcardService.GetAllList());
        
        [HttpGet("{count:int}")]
        public async Task<IActionResult> GetTopPriced(int count) => CreateActionResult(await stockcardService.GetAllList());
        
        [RoleAuthorize("Admin", "Editor")] // Sadece Admin ve Editor oluşturabilir
        [HttpPost]
        public async Task<IActionResult> Create(CreateStockCardRequest request) => CreateActionResult(await stockcardService.CreateAsync(request));
        
        [RoleAuthorize("Admin", "Editor")] // Sadece Admin ve Editor güncelleyebilir
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateStockCardRequest request) => CreateActionResult(await stockcardService.UpdateAsync(id, request));

        [HttpGet("{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetPagedAll(int pageNumber, int pageSize) =>
        CreateActionResult(await stockcardService.GetPagedAllListAsync(pageNumber, pageSize));

        [RoleAuthorize("Admin", "Editor")] // Sadece Admin ve Editor silebilir
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) => CreateActionResult(await stockcardService.DeleteAsync(id));
    }
}