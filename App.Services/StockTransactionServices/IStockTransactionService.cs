namespace App.Services.StockTransactionServices
{
    public interface IStockTransactionService
    {
        Task<ServiceResult<List<StockTransactionDto>>> GetAllAsync();
        Task<ServiceResult<StockTransactionDto?>> GetByIdAsync(int id);
        Task<ServiceResult<CreateStockTransactionResponse>> CreateAsync(CreateStockTransactionRequest request);
        Task<ServiceResult> UpdateAsync(int id, UpdateStockTransactionRequest request);
        Task<ServiceResult> DeleteAsync(int id);

    }
}