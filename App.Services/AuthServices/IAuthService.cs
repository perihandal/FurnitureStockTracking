﻿using System.Threading.Tasks;
using System.Collections.Generic;
using App.Repositories.Users;

namespace App.Services.Auth
{
    public interface IAuthService
    {
        Task<(bool Success, User? User, List<string> Roles, string? Error)> AuthenticateAsync(string username, string password);
        Task<(bool Success, int? UserId, string? Error)> RegisterAsync(string username, string fullName, string email, string password);
        Task<(bool Success, string? RefreshToken, DateTime? ExpiresAtUtc, string? Error)> IssueRefreshTokenAsync(int userId);
        Task<(bool Success, User? User, List<string> Roles, string? Error)> ValidateRefreshTokenAsync(string refreshToken);
        Task<(bool Success, string? Error)> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    }
}
