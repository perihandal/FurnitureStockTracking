using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.ProductServices
{
    public interface IProductService
    {
        Task<ServiceResult<List<ProductDto>>> GetTopPriceASync(int count);
        Task<ServiceResult<List<ProductDto>>> GetAllList();
        Task<ServiceResult<ProductDto?>> GetByIdAsync(int id);
        Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request);
        Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request);
    }
}
