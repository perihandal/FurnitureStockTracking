using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.PriceDefinitions
{
    public class PriceDefinitionRepository : GenericRepository<PriceDefinition>, IPriceDefinitionRepository
    {
        private readonly AppDbContext _context;

        public PriceDefinitionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<PriceDefinition>> GetAllWithDetailsAsync()
        {
            return await _context.PriceDefinitions
                .Include(pd => pd.User)
                .Include(pd => pd.StockCard)
                .ToListAsync();
        }

        public async Task<PriceDefinition?> GetAllWithDetailsAsync(int id)
        {
            return await _context.PriceDefinitions
                .Include(pd => pd.User)
                .Include(pd => pd.StockCard)
                .FirstOrDefaultAsync(pd => pd.Id == id);
        }

        public async Task<PriceDefinition?> GetByStockCardAndTypeAsync(int stockCardId, PriceType priceType)
        {
            return await _context.PriceDefinitions
                .FirstOrDefaultAsync(pd => pd.StockCardId == stockCardId && pd.PriceType == priceType);
        }
    }
}
