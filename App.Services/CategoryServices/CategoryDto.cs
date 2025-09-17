
namespace App.Services.CategoryServices
{
    public record class CategoryDto(
    int Id,
    string Code,
    bool IsActive,  
    string Name,
    int CompanyId,
    string CompanyName,
    int BranchId,
    string BranchName,
    string UserName,
    DateTime CreateDate
        );

}
