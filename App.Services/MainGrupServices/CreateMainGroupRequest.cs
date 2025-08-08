using System;

namespace App.Services.MainGroupServices
{
    public record class CreateMainGroupRequest(
        string Code,
        string Name,
        int UserId
       
    );
}
