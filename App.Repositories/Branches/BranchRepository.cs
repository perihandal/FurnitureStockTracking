using App.Repositories.Branches;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories.Branches
{
    public class BranchRepository(AppDbContext context) : GenericRepository<Branch>(context), IBranchRepository
    {
        public async Task<List<Branch>> GetAllWithDetailsAsync()
        {
            return await context.Branches
                .Include(b => b.Company)              // Şube ile ilişkili Şirket
                .Include(b => b.Warehouses)          // Şube ile ilişkili Depolar
                .Include(b => b.StockCards)          // Şube ile ilişkili Stok Kartları
                .Include(b=> b.User)
                .ToListAsync();
        }
    }
}
