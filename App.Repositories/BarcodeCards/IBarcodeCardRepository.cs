namespace App.Repositories.BarcodeCards
{
    public interface IBarcodeCardRepository:IGenericRepository<BarcodeCard>
    {
        Task<List<BarcodeCard>> GetAllWithDetailsAsync();
        Task<List<BarcodeCard>> GetByStockCardIdAsync(int stockCardId);
        Task<BarcodeCard?> GetByBarcodeCodeAsync(string barcodeCode);
        Task<BarcodeCard?> GetDefaultByStockCardIdAsync(int stockCardId);
        Task<List<BarcodeCard>> GetByBarcodeTypeAsync(BarcodeType barcodeType);
        Task<List<BarcodeCard>> GetByCompanyIdAsync(int companyId);
        Task<List<BarcodeCard>> GetByBranchIdAsync(int branchId);
        Task<bool> ExistsByBarcodeCodeAsync(string barcodeCode);
        Task<int> GetCountByStockCardIdAsync(int stockCardId);
    }
}