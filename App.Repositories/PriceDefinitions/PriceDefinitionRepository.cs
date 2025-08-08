using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.PriceDefinitions
{
    public class PriceDefinitionRepository(AppDbContext context) : GenericRepository<PriceDefinition>(context), IPriceDefinitionRepository
    {
        public async Task<List<PriceDefinition>> GetAllWithDetailsAsync()
        {
            return await context.PriceDefinitions
                .Include(pd => pd.User)  
                .Include(pd => pd.StockCard)  
                .ToListAsync();  
        }
        public async Task<PriceDefinition> GetAllWithDetailsAsync(int id)
        {
            return await context.PriceDefinitions
                  .Include(pd => pd.User)  
                  .Include(pd => pd.StockCard) 
                  .FirstOrDefaultAsync(pd => pd.Id == id);  
        }
    }
}
