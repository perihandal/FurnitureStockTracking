using App.Repositories;
using App.Repositories.StockCards;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.StockCards
{
    public class StockCardRepository(AppDbContext context) : GenericRepository<StockCard>(context), IStockCardRepository
    {
        public async Task<List<StockCard>> GetTopPriceProductsAsync(int count)
        {
            return await context.StockCards
                .Include(sc => sc.PriceDefinitions) // Price'ı çekmek için
                .ToListAsync();
        }

        public async Task<List<StockCard>> GetAllWithDetailsAsync()
        {
            return await context.StockCards
                .Include(s => s.Company)
                .Include(s => s.Branch)
                .Include(s => s.MainGroup)
                .Include(s => s.SubGroup)
                .Include(s => s.Category)
                .Include(s => s.User)
              //  .Include(s => s.PriceDefinitions)
                .Include(s => s.BarcodeCards)
                .ToListAsync();
        }


    }
}
