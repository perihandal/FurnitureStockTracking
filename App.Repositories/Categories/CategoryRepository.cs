using App.Repositories.StockCards;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Categories
{
    public class CategoryRepository(AppDbContext context) : GenericRepository<Category>(context), ICategoryRepository
    {
        public async Task<List<Category>> GetAllWithDetailsAsync()
        {
            return await context.Categories
                .Include(s => s.Company)
                .Include(s => s.Branch)
                .Include(s => s.User)
                .ToListAsync();
        }
    }
}
