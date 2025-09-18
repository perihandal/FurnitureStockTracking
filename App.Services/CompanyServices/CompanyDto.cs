using App.Repositories.Branches;
using App.Repositories.Warehouses;

namespace App.Services.CompanyServices
{
    public record class CompanyDto(
        int Id,
        string Code,
        string Name,
        string TaxNumber,
        string Address,
        string Phone,
        bool IsActive,
       // int UserId,
        string UserName,
        List<string> BranchNames,
        List<string> WarehouseNames
    );

}