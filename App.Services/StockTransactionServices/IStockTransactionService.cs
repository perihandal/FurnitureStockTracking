namespace App.Services.StockTransactionServices
{
    public interface IStockTransactionService
    {
        Task<ServiceResult> UpdateAsync(int id, UpdateStockTransactionRequest request);
        Task<ServiceResult<CreateStockTransactionResponse>> CreateAsync(CreateStockTransactionRequest request);
        Task<ServiceResult<StockTransactionDto?>> GetByIdAsync(int id);
        Task<ServiceResult<List<StockTransactionDto>>> GetAllListAsync();

    }
}