using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.StockCards
{
    public interface IStockCardRepository :IGenericRepository<StockCard>
    {
        //Task<List<StockCard>> GetTopPriceProductsAsync(int count);
    }
}
