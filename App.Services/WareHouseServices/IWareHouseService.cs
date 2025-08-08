using App.Services.CompanyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.WareHouseServices
{
    public interface IWareHouseService
    {
        Task<ServiceResult<CreateWareHouseResponse>> CreateAsync(CreateWareHouseRequest request);
        Task<ServiceResult> UpdateAsync(int id, UpdateWareHouseRequest request);
        Task<ServiceResult<List<WareHouseDto>>> GetAllList();
    }
}
