using App.Repositories.PriceDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.PriceHistories
{
    public class PriceHistoryRepository(AppDbContext context) : GenericRepository<PriceHistory>(context), IPriceHistoryRepository
    {
    }

}
