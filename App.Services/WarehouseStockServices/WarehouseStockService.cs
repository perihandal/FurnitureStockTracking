using App.Services;
using System.Net;

namespace App.Services.WareHouseStockServices
{
    public class WarehouseStockService : IWarehouseStockService
    {
        private readonly IWarehouseStockRepository _warehouseStockRepository;

        public WarehouseStockService(IWarehouseStockRepository warehouseStockRepository)
        {
            _warehouseStockRepository = warehouseStockRepository;
        }

        public async Task<ServiceResult<List<WarehouseStockDto>>> GetAllAsync()
        {
            var warehouseStocks = await _warehouseStockRepository.GetAllWithDetailsAsync();

            var dtos = warehouseStocks.Select(ws => new WarehouseStockDto
            {
                WarehouseId = ws.WarehouseId,
                WarehouseName = ws.Warehouse.Name,
                StockCardId = ws.StockCardId,
                StockCardName = ws.StockCard.Name,
                Quantity = ws.Quantity
            }).ToList();

            return ServiceResult<List<WarehouseStockDto>>.Success(dtos);
        }

        public async Task<ServiceResult<WarehouseStockDto?>> GetByWarehouseAndStockCardAsync(int warehouseId, int stockCardId)
        {
            var ws = await _warehouseStockRepository.GetByWarehouseAndStockCardAsync(warehouseId, stockCardId);
            if (ws == null)
                return ServiceResult<WarehouseStockDto?>.Fail("Warehouse stock not found", HttpStatusCode.NotFound);

            var dto = new WarehouseStockDto
            {
                WarehouseId = ws.WarehouseId,
                WarehouseName = ws.Warehouse.Name,
                StockCardId = ws.StockCardId,
                StockCardName = ws.StockCard.Name,
                Quantity = ws.Quantity
            };

            return ServiceResult<WarehouseStockDto?>.Success(dto);
        }
    }
}
