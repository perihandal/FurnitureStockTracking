using App.Services.WareHouseStockServices;
using App.Services;

public interface IWarehouseStockService
{
    Task<ServiceResult<List<WarehouseStockDto>>> GetAllAsync();
    Task<ServiceResult<WarehouseStockDto?>> GetByWarehouseAndStockCardAsync(int warehouseId, int stockCardId);
}
