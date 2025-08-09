using App.Repositories.PriceDefinitions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.PriceHistories
{
    public class PriceHistoryRepository(AppDbContext context) : GenericRepository<PriceHistory>(context), IPriceHistoryRepository
    {
        public async Task AddAsync(PriceHistory priceHistory)
        {
            await context.PriceHistories.AddAsync(priceHistory);
        }

        public async Task<List<PriceHistory>> GetAllWithDetailsAsync()
        {
            return await context.PriceHistories
                .Include(pd => pd.PriceDefinition)
                .ThenInclude(pd => pd.StockCard)
                .ToListAsync();
        }

        public async Task<PriceHistory> GetAllWithDetailsAsync(int id)
        {
            return await context.PriceHistories
               .Include(pd => pd.PriceDefinition)
                 .ThenInclude(pd => pd.StockCard)
                  .FirstOrDefaultAsync(pd => pd.Id == id);
        }
    }

}
