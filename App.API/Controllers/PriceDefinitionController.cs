using App.Services.PriceDefinitionServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    public class PriceDefinitionController : CustomBaseController
    {
        private readonly IPriceDefinitionService _priceDefinitionService;

        public PriceDefinitionController(IPriceDefinitionService priceDefinitionService)
        {
            _priceDefinitionService = priceDefinitionService;
        }

        // Tüm PriceDefinition'ları listeleme
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _priceDefinitionService.GetAllListAsync();
            return CreateActionResult(result);
        }

        // ID'ye göre PriceDefinition getirme
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
            => CreateActionResult(await _priceDefinitionService.GetByIdAsync(id));

        // Yeni PriceDefinition oluşturma
        [HttpPost]
        public async Task<IActionResult> Create(CreatePriceDefinitionRequest request)
            => CreateActionResult(await _priceDefinitionService.CreateAsync(request));

        // Mevcut PriceDefinition güncelleme
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePriceDefinitionRequest request)
            => CreateActionResult(await _priceDefinitionService.UpdateAsync(id, request));

        // PriceDefinition silme
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => CreateActionResult(await _priceDefinitionService.DeleteAsync(id));
    }
}
