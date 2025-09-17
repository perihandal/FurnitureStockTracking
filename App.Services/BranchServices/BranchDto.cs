namespace App.Services.BranchServices
{
    public record class BranchDto
    (
        int Id,
        string Code,
        string Name,
        string Address,
        string Phone,
        bool IsActive,
        int CompanyId,
        string CompanyName,
        string UserFullName,
        List<string> WarehouseNames,
        List<string> StockCardCodes
    );
}