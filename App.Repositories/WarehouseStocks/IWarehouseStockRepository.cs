using App.Repositories.WarehouseStocks;
using App.Repositories;

public interface IWarehouseStockRepository : IGenericRepository<WarehouseStock>
{
    Task<WarehouseStock?> GetByWarehouseAndStockCardAsync(int warehouseId, int stockCardId);
    Task<List<WarehouseStock>> GetAllWithDetailsAsync();
}
