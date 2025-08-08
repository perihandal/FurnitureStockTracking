namespace App.Services.MainGroupServices
{
    public record class UpdateMainGroupRequest
    (

         string Code,
         string Name,
         int UserId,
         bool IsActive
    );
}