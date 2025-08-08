using System.Threading.Tasks;

namespace App.Services.SubGroupServices
{
    public interface ISubGroupService
    {
        Task<ServiceResult> DeleteAsync(int id);
        Task<ServiceResult> UpdateAsync(int id, UpdateSubGroupRequest request);
        Task<ServiceResult<CreateSubGroupResponse>> CreateAsync(CreateSubGroupRequest request);
        Task<ServiceResult<SubGroupDto?>> GetByIdAsync(int id);
        Task<ServiceResult<List<SubGroupDto>>> GetAllListAsync();
    }
}