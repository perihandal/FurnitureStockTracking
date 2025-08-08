namespace App.Services.SubGroupServices
{
    public record UpdateSubGroupRequest(
      string Code,
      string Name,
      int UserId,
      int MainGroupId,
      bool IsActive
  );
}