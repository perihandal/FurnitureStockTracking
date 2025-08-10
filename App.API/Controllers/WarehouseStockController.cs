using App.Services.WareHouseStockServices;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    public class WarehouseStockController : CustomBaseController
    {
        private readonly IWarehouseStockService _warehouseStockService;

        public WarehouseStockController(IWarehouseStockService warehouseStockService)
        {
            _warehouseStockService = warehouseStockService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => CreateActionResult(await _warehouseStockService.GetAllAsync());

        // Depoya ve stok kartına göre stok bilgisi sorgulama
        [HttpGet("by-warehouse-stockcard")]
        public async Task<IActionResult> GetByWarehouseAndStockCard(int warehouseId, int stockCardId)
            => CreateActionResult(await _warehouseStockService.GetByWarehouseAndStockCardAsync(warehouseId, stockCardId));
    }
}
