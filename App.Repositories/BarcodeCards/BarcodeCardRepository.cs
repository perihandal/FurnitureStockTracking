using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.BarcodeCards
{
    public class BarcodeCardRepository(AppDbContext _context) : GenericRepository<BarcodeCard>(_context), IBarcodeCardRepository
    {
        public async Task<List<BarcodeCard>> GetAllWithDetailsAsync()
        {
            return await _context.BarcodeCards
                .Include(bc => bc.StockCard)
                .Include(bc => bc.User)
                .Include(bc => bc.Branch)
                .Include(bc => bc.Company)
                .OrderBy(bc => bc.StockCardId)
                .ThenByDescending(bc => bc.IsDefault)
                .ThenBy(bc => bc.CreateDate)
                .ToListAsync();
        }

        public async Task<List<BarcodeCard>> GetByStockCardIdAsync(int stockCardId)
        {
            return await _context.BarcodeCards
                .Include(bc => bc.StockCard)
                .Include(bc => bc.User)
                .Include(bc => bc.Branch)
                .Include(bc => bc.Company)
                .Where(bc => bc.StockCardId == stockCardId)
                .OrderByDescending(bc => bc.IsDefault)
                .ThenBy(bc => bc.CreateDate)
                .ToListAsync();
        }

        public async Task<BarcodeCard?> GetByBarcodeCodeAsync(string barcodeCode)
        {
            return await _context.BarcodeCards
                .Include(bc => bc.StockCard)
                .Include(bc => bc.User)
                .Include(bc => bc.Branch)
                .Include(bc => bc.Company)
                .FirstOrDefaultAsync(bc => bc.BarcodeCode == barcodeCode);
        }

        public async Task<BarcodeCard?> GetDefaultByStockCardIdAsync(int stockCardId)
        {
            return await _context.BarcodeCards
                .Include(bc => bc.StockCard)
                .Include(bc => bc.User)
                .Include(bc => bc.Branch)
                .Include(bc => bc.Company)
                .FirstOrDefaultAsync(bc => bc.StockCardId == stockCardId && bc.IsDefault);
        }

        public async Task<List<BarcodeCard>> GetByBarcodeTypeAsync(BarcodeType barcodeType)
        {
            return await _context.BarcodeCards
                .Include(bc => bc.StockCard)
                .Include(bc => bc.User)
                .Include(bc => bc.Branch)
                .Include(bc => bc.Company)
                .Where(bc => bc.BarcodeType == barcodeType)
                .OrderBy(bc => bc.StockCardId)
                .ThenBy(bc => bc.CreateDate)
                .ToListAsync();
        }

        public async Task<List<BarcodeCard>> GetByCompanyIdAsync(int companyId)
        {
            return await _context.BarcodeCards
                .Include(bc => bc.StockCard)
                .Include(bc => bc.User)
                .Include(bc => bc.Branch)
                .Include(bc => bc.Company)
                .Where(bc => bc.CompanyId == companyId)
                .OrderBy(bc => bc.StockCardId)
                .ThenByDescending(bc => bc.IsDefault)
                .ThenBy(bc => bc.CreateDate)
                .ToListAsync();
        }

        public async Task<List<BarcodeCard>> GetByBranchIdAsync(int branchId)
        {
            return await _context.BarcodeCards
                .Include(bc => bc.StockCard)
                .Include(bc => bc.User)
                .Include(bc => bc.Branch)
                .Include(bc => bc.Company)
                .Where(bc => bc.BranchId == branchId)
                .OrderBy(bc => bc.StockCardId)
                .ThenByDescending(bc => bc.IsDefault)
                .ThenBy(bc => bc.CreateDate)
                .ToListAsync();
        }

        public async Task<bool> ExistsByBarcodeCodeAsync(string barcodeCode)
        {
            return await _context.BarcodeCards
                .AnyAsync(bc => bc.BarcodeCode == barcodeCode);
        }

        public async Task<int> GetCountByStockCardIdAsync(int stockCardId)
        {
            return await _context.BarcodeCards
                .CountAsync(bc => bc.StockCardId == stockCardId);
        }

        // Ek metodlar - performans için
        public async Task<List<BarcodeCard>> GetByStockCardIdWithoutDetailsAsync(int stockCardId)
        {
            return await _context.BarcodeCards
                .Where(bc => bc.StockCardId == stockCardId)
                .OrderByDescending(bc => bc.IsDefault)
                .ThenBy(bc => bc.CreateDate)
                .ToListAsync();
        }

        public async Task<List<BarcodeCard>> GetActiveBarcodesAsync()
        {
            return await _context.BarcodeCards
                .Include(bc => bc.StockCard)
                .Where(bc => bc.StockCard != null) // StockCard silinmemiş olanlar
                .OrderBy(bc => bc.StockCardId)
                .ThenByDescending(bc => bc.IsDefault)
                .ThenBy(bc => bc.CreateDate)
                .ToListAsync();
        }

        public async Task<Dictionary<int, int>> GetBarcodeCountsByStockCardAsync(List<int> stockCardIds)
        {
            return await _context.BarcodeCards
                .Where(bc => stockCardIds.Contains(bc.StockCardId))
                .GroupBy(bc => bc.StockCardId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<List<BarcodeCard>> SearchByBarcodeCodeAsync(string searchTerm, int take = 50)
        {
            return await _context.BarcodeCards
                .Include(bc => bc.StockCard)
                .Include(bc => bc.Company)
                .Include(bc => bc.Branch)
                .Where(bc => bc.BarcodeCode.Contains(searchTerm))
                .OrderBy(bc => bc.BarcodeCode)
                .Take(take)
                .ToListAsync();
        }

        // Bulk operations
        public async Task BulkUpdateDefaultStatusAsync(int stockCardId, int newDefaultBarcodeId)
        {
            var barcodes = await _context.BarcodeCards
                .Where(bc => bc.StockCardId == stockCardId)
                .ToListAsync();

            foreach (var barcode in barcodes)
            {
                barcode.IsDefault = barcode.Id == newDefaultBarcodeId;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<BarcodeCard>> GetExpiredBarcodesAsync(DateTime beforeDate)
        {
            return await _context.BarcodeCards
                .Include(bc => bc.StockCard)
                .Include(bc => bc.Company)
                .Include(bc => bc.Branch)
                .Where(bc => bc.CreateDate < beforeDate)
                .OrderBy(bc => bc.CreateDate)
                .ToListAsync();
        }

        // Statistik metodları
        public async Task<Dictionary<BarcodeType, int>> GetBarcodeTypeStatisticsAsync()
        {
            return await _context.BarcodeCards
                .GroupBy(bc => bc.BarcodeType)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<int, int>> GetCompanyBarcodeStatisticsAsync()
        {
            return await _context.BarcodeCards
                .GroupBy(bc => bc.CompanyId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<int, int>> GetBranchBarcodeStatisticsAsync()
        {
            return await _context.BarcodeCards
                .GroupBy(bc => bc.BranchId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        // Özel sorgular
        public async Task<List<BarcodeCard>> GetDuplicateBarcodesAsync()
        {
            var duplicateCodes = await _context.BarcodeCards
                .GroupBy(bc => bc.BarcodeCode)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
            .ToListAsync();

            return await _context.BarcodeCards
                .Include(bc => bc.StockCard)
                .Include(bc => bc.Company)
                .Include(bc => bc.Branch)
                .Where(bc => duplicateCodes.Contains(bc.BarcodeCode))
                .OrderBy(bc => bc.BarcodeCode)
                .ThenBy(bc => bc.CreateDate)
                .ToListAsync();
        }

        public async Task<List<BarcodeCard>> GetStockCardsWithMultipleDefaultBarcodesAsync()
        {
            var stockCardIdsWithMultipleDefaults = await _context.BarcodeCards
                .Where(bc => bc.IsDefault)
                .GroupBy(bc => bc.StockCardId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
            .ToListAsync();

            return await _context.BarcodeCards
                .Include(bc => bc.StockCard)
                .Include(bc => bc.Company)
                .Include(bc => bc.Branch)
                .Where(bc => stockCardIdsWithMultipleDefaults.Contains(bc.StockCardId) && bc.IsDefault)
                .OrderBy(bc => bc.StockCardId)
                .ThenBy(bc => bc.CreateDate)
                .ToListAsync();
        }
    }
}
