using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace App.API.Auth
{
    public class TokenService : ITokenService
    {
        public string CreateToken(int userId, string username, string fullName, IEnumerable<string> roles, int? companyId, int? branchId, TokenOptions options, out DateTime expiresAtUtc)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim("name", fullName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Şirket ve şube bilgilerini token'a ekle (Editor ve User rolleri için)
            if (companyId.HasValue)
            {
                claims.Add(new Claim("companyId", companyId.Value.ToString()));
            }

            if (branchId.HasValue)
            {
                claims.Add(new Claim("branchId", branchId.Value.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            expiresAtUtc = DateTime.UtcNow.AddMinutes(options.AccessTokenMinutes);

            var token = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAtUtc,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}