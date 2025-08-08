
namespace App.Services.CategoryServices
{
    public record class CategoryDto(
    string Code,
    string Name,
    string CompanyName,
    string BranchName,
    string UserName,
    DateTime CreateDate
        );

}
