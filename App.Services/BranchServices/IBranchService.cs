using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.BranchServices
{
    public interface IBranchService
    {
        Task<ServiceResult<BranchDto>> GetByIdAsync(int id);
        Task<ServiceResult<List<BranchDto>>> GetAllList();
        Task<ServiceResult> UpdateAsync(int id, UpdateBranchRequest request);
        Task<ServiceResult<CreateBranchResponse>> CreateAsync(CreateBranchRequest request);
    }
}
