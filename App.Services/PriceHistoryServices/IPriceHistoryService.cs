using System.Threading.Tasks;

namespace App.Services.PriceDefinitionServices
{
    public interface IPriceHistoryService
    {
        Task<ServiceResult<List<PriceHistoryDto>>> GetAllListAsync();
        Task<ServiceResult<PriceHistoryDto?>> GetByIdAsync(int id);
    }
}