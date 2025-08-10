using App.Repositories.WarehouseStocks;
using App.Repositories;
using Microsoft.EntityFrameworkCore;

public class WarehouseStockRepository : GenericRepository<WarehouseStock>, IWarehouseStockRepository
{
    private readonly AppDbContext _context;

    public WarehouseStockRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<WarehouseStock?> GetByWarehouseAndStockCardAsync(int warehouseId, int stockCardId)
    {
        return await _context.WarehouseStocks
            .Include(ws => ws.Warehouse)
            .Include(ws => ws.StockCard)
            .FirstOrDefaultAsync(ws => ws.WarehouseId == warehouseId && ws.StockCardId == stockCardId);
    }


    public async Task<List<WarehouseStock>> GetAllWithDetailsAsync()
    {
        return await _context.WarehouseStocks
            .Include(ws => ws.Warehouse)
            .Include(ws => ws.StockCard)
            .ToListAsync();
    }
}
