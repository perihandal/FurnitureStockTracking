
using App.Repositories.Warehouses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Warehouses
{
    public class WarehouseRepository(AppDbContext context) : GenericRepository<Warehouse>(context), IWarehouseRepository
    {
        public async Task<List<Warehouse>> GetAllWithDetailsAsync()
        {
            return await context.Warehouses
                .Include(w => w.Company)
                .Include(w => w.Branch)
                //.Include(w => w.CreateUser)
                .ToListAsync();
        }

        public async Task<Warehouse?> GetByCodeAsync(string code)
        {
            return await context.Warehouses.FirstOrDefaultAsync(w => w.Code == code);
        }

        public async Task<Warehouse?> GetByNameAsync(string name)
        {
            return await context.Warehouses.FirstOrDefaultAsync(w => w.Name == name);
        }
    }
}
