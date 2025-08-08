namespace App.Services.BranchServices
{
    public record class UpdateBranchRequest
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