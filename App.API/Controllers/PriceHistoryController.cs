using App.Services.PriceDefinitionServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    public class PriceHistoryController : CustomBaseController
    {
        private readonly IPriceHistoryService _priceHistoryService;

        public PriceHistoryController(IPriceHistoryService priceHistoryService)
        {
            _priceHistoryService = priceHistoryService;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _priceHistoryService.GetAllListAsync();
            return CreateActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
            => CreateActionResult(await _priceHistoryService.GetByIdAsync(id));

    }
}
