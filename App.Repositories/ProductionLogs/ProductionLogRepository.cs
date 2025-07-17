using App.Repositories.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.ProductionLogs
{
    public class ProductionLogRepository(AppDbContext context) : GenericRepository<ProductionLog>(context), IProductionLogRepository
    {
    }
}
