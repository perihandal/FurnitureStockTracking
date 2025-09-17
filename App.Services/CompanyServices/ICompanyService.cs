using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.CompanyServices
{
    public interface ICompanyService
    {
        Task<ServiceResult<CreateCompanyResponse>> CreateAsync(CreateCompanyRequest request);
        Task<ServiceResult> UpdateAsync(int id, UpdateCompanyRequest request);
        Task<ServiceResult<List<CompanyDto>>> GetAllList();
        Task<ServiceResult> DeleteAsync(int id);

    }
}
