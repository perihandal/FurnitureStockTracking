namespace App.Services.PriceDefinitionServices
{
    public interface IPriceDefinitionService
    {
        Task<ServiceResult<List<PriceDefinitionDto>>> GetAllListAsync();
        Task<ServiceResult<PriceDefinitionDto?>> GetByIdAsync(int id);
        Task<ServiceResult<CreatePriceDefinitionResponse>> CreateAsync(CreatePriceDefinitionRequest request);
        Task<ServiceResult> UpdateAsync(int id, UpdatePriceDefinitionRequest request);
        Task<ServiceResult> DeleteAsync(int id);
    }
}