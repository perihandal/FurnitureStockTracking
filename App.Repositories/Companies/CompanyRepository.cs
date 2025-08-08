using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Companies
{
    public class CompanyRepository(AppDbContext context) : GenericRepository<Company>(context), ICompanyRepository
    {
        public async Task<List<Company>> GetAllWithDetailsAsync()
        {
            return await context.Companies
                .Include(c => c.Branches)
                .Include(c => c.StockCards)
                .Include(c => c.Warehouses)
                .ToListAsync();
        }

    }
}
