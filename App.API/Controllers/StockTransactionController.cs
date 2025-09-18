using App.API.Auth;
using App.Services.StockTransactionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [CompanyAuthorize]
    [Authorize(Roles = "Admin,Editor,User")]
    public class StockTransactionController : CustomBaseController
    {
        private readonly IStockTransactionService _stockTransactionService;

        public StockTransactionController(IStockTransactionService stockTransactionService)
        {
            _stockTransactionService = stockTransactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => CreateActionResult(await _stockTransactionService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
            => CreateActionResult(await _stockTransactionService.GetByIdAsync(id));

        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Create(CreateStockTransactionRequest request)
            => CreateActionResult(await _stockTransactionService.CreateAsync(request));

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Update(int id, UpdateStockTransactionRequest request)
            => CreateActionResult(await _stockTransactionService.UpdateAsync(id, request));

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Delete(int id)
            => CreateActionResult(await _stockTransactionService.DeleteAsync(id));
    }
}
