namespace App.Services.MainGroupServices
{
    public interface IMainGroupService
    {
        Task<ServiceResult<List<MainGroupDto>>> GetAllListAsync();

        // ID'ye göre MainGroup getirme
        Task<ServiceResult<MainGroupDto?>> GetByIdAsync(int id);

        // Yeni MainGroup oluşturma
        Task<ServiceResult<CreateMainGroupResponse>> CreateAsync(CreateMainGroupRequest request);

        // MainGroup güncelleme
        Task<ServiceResult> UpdateAsync(int id, UpdateMainGroupRequest request);

        // MainGroup silme
        Task<ServiceResult> DeleteAsync(int id);
    }
}