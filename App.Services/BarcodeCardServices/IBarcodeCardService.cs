using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Repositories.BarcodeCards;

namespace App.Services.BarcodeCardServices
{
    public interface IBarcodeCardService
    {

        Task<ServiceResult<List<BarcodeCardDto>>> GetAllListAsync();
        Task<ServiceResult<BarcodeCardDto?>> GetByIdAsync(int id);
        Task<ServiceResult<List<BarcodeCardDto>>> GetByStockCardIdAsync(int stockCardId);
        Task<ServiceResult<CreateBarcodeCardResponse>> CreateAsync(CreateBarcodeCardRequest request);
        Task<ServiceResult> UpdateAsync(int id, UpdateBarcodeCardRequest request);
        Task<ServiceResult> DeleteAsync(int id);
        Task<ServiceResult> SetAsDefaultAsync(int id);
        Task<ServiceResult> ValidateBarcodeAsync(string barcodeCode, BarcodeType barcodeType);
    }
}