namespace App.Services.SubGroupServices
{
    public record CreateSubGroupRequest(
     string Code,
     string Name,
     int UserId,
     int MainGroupId
 );

}