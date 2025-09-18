using App.API.Auth;
using App.Services.PriceDefinitionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [CompanyAuthorize] // Editor ve User için companyId kontrolü
    [Authorize(Roles = "Admin,Editor,User")]
    public class PriceHistoryController : CustomBaseController
    {
        private readonly IPriceHistoryService _priceHistoryService;

        public PriceHistoryController(IPriceHistoryService priceHistoryService)
        {
            _priceHistoryService = priceHistoryService;
        }

        
        [HttpGet]
        // User ve Editor sadece kendi company/branch ile okuma yapabilir
        public async Task<IActionResult> GetAll()
        {
            // companyId ve branchId kontrolü service layer'da yapılmalı
            var result = await _priceHistoryService.GetAllListAsync();
            return CreateActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // companyId ve branchId kontrolü service layer'da yapılmalı
            var result = await _priceHistoryService.GetByIdAsync(id);
            return CreateActionResult(result);
        }

    }
}
