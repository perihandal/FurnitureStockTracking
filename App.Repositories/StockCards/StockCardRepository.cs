using App.Repositories;
using App.Repositories.StockCards;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AApp.Repositories.StockCards
{
    public class StockCardRepository(AppDbContext context) : GenericRepository<StockCard>(context), IStockCardRepository
    {
        //public async Task<List<StockCard>> GetTopPriceProductsAsync(int count)
        //{
        //    return await context.StockCard
        //        .Include(sc => sc.PriceDefinitions) // Price'ı çekmek için
        //        .ToListAsync();
        //}

    }
}
