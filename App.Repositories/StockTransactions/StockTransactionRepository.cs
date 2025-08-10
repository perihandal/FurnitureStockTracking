using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.StockTransactions
{
    public class StockTransactionRepository(AppDbContext context) : GenericRepository<StockTransaction>(context), IStockTransactionRepository
    {
        public async Task<List<StockTransaction>> GetAllWithDetailsAsync()
        {
            return await context.Set<StockTransaction>()
                .Include(st => st.StockCard)
                .Include(st => st.Warehouse)
                .Include(st => st.FromWarehouse)
                .Include(st => st.ToWarehouse)
                .Include(st => st.User)
                .ToListAsync();
        }

        public async Task<StockTransaction?> GetByIdWithDetailsAsync(int id)
        {
            return await context.Set<StockTransaction>()
                .Include(st => st.StockCard)
                .Include(st => st.Warehouse)
                .Include(st => st.FromWarehouse)
                .Include(st => st.ToWarehouse)
                .Include(st => st.User)
                .FirstOrDefaultAsync(st => st.Id == id);
        }
    }
}
