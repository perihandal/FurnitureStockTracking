using App.API.Auth;
using App.Services.PriceDefinitionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [CompanyAuthorize]
    [Authorize(Roles = "Admin,Editor,User")]
    public class PriceDefinitionController : CustomBaseController
    {
        private readonly IPriceDefinitionService _priceDefinitionService;

        public PriceDefinitionController(IPriceDefinitionService priceDefinitionService)
        {
            _priceDefinitionService = priceDefinitionService;
        }

        // Tüm PriceDefinition'ları listeleme
        [HttpGet]
        // User ve Editor sadece kendi company/branch ile okuma yapabilir
        public async Task<IActionResult> GetAll()
        {
            var result = await _priceDefinitionService.GetAllListAsync();
            return CreateActionResult(result);
        }

        // ID'ye göre PriceDefinition getirme
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _priceDefinitionService.GetByIdAsync(id);
            return CreateActionResult(result);
        }

        // Yeni PriceDefinition oluşturma
        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Create(CreatePriceDefinitionRequest request)
        {
            var result = await _priceDefinitionService.CreateAsync(request);
            return CreateActionResult(result);
        }

        // Mevcut PriceDefinition güncelleme
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Update(int id, UpdatePriceDefinitionRequest request)
        {
            var result = await _priceDefinitionService.UpdateAsync(id, request);
            return CreateActionResult(result);
        }

        // PriceDefinition silme
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _priceDefinitionService.DeleteAsync(id);
            return CreateActionResult(result);
        }
    }
}
