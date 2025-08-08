namespace App.Services.SubGroupServices
{
    public record SubGroupDto(
        int Id,
        string Code,
        string Name,
        bool IsActive,
        int? UserId,
        int MainGroupId
    );
}