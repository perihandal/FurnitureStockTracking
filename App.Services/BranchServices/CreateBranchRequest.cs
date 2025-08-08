namespace App.Services.BranchServices
{
    public record class CreateBranchRequest
    (
        string Code,
        string Name,
        string Address,
        string Phone,
        bool IsActive,
        int CompanyId,
        int UserId
    );
}