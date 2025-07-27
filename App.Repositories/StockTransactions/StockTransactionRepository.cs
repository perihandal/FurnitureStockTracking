using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.StockTransactions
{
    public class StockTransactionRepository(AppDbContext context) : GenericRepository<StockTransaction>(context), IStockTransactionRepository
    {
    }
}
