namespace App.Repositories.PriceHistories
{
    public interface IPriceHistoryRepository
    {
        Task AddAsync(PriceHistory priceHistory);
        Task<List<PriceHistory>> GetAllWithDetailsAsync();

        Task<PriceHistory> GetAllWithDetailsAsync(int id);
    }
}