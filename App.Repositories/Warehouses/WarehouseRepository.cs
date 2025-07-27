using App.Repositories.Warehauses;
using App.Repositories.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Warehauses
{
    public class WarehauseRepository(AppDbContext context) : GenericRepository<Warehouse>(context), IWarehauseRepository
    {
    }
}
