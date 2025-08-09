using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.StockTransactions
{
   public interface IStockTransactionRepository : IGenericRepository<StockTransaction>
    {
        Task<List<StockTransaction>> GetAllWithDetailsAsync();
        Task<StockTransaction?> GetByIdWithDetailsAsync(int id);
    }
}
