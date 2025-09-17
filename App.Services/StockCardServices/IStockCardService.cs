using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.StockCardServices
{
    public interface IStockCardService
    {
        //Task<ServiceResult<List<StockCardDto>>> GetTopPriceASync(int count);
        Task<ServiceResult<List<StockCardDto>>> GetAllList();
        //Task<ServiceResult<StockCardDto?>> GetByIdAsync(int id);
        Task<ServiceResult<CreateStockCardResponse>> CreateAsync(CreateStockCardRequest request);
        Task<ServiceResult> UpdateAsync(int id, UpdateStockCardRequest request);
        Task<ServiceResult<List<StockCardDto>>> GetPagedAllListAsync(int pageNumber, int pageSize);
        Task<ServiceResult> DeleteAsync(int id);

    }
}
