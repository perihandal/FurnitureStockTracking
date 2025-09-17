using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace App.API.Auth
{
    public interface ITokenService
    {
        string CreateToken(int userId, string username, string fullName, IEnumerable<string> roles, int? companyId, int? branchId, TokenOptions options, out DateTime expiresAtUtc);
    }
}