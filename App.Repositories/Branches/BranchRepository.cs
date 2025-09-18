using App.Repositories.Branches;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories.Branches
{
    public class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {
        private readonly AppDbContext _context;

        public BranchRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public new async Task<Branch?> GetByIdAsync(int id)
        {
            return await _context.Branches
                .Include(b => b.Company)              // Şube ile ilişkili Şirket
                .Include(b => b.Warehouses)          // Şube ile ilişkili Depolar
                .Include(b => b.StockCards)          // Şube ile ilişkili Stok Kartları
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Branch>> GetAllWithDetailsAsync()
        {
            return await _context.Branches
                .Include(b => b.Company)              // Şube ile ilişkili Şirket
                .Include(b => b.Warehouses)          // Şube ile ilişkili Depolar
                .Include(b => b.StockCards)          // Şube ile ilişkili Stok Kartları
                .Include(b=> b.User)
                .ToListAsync();
        }
    }
}
