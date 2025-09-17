using App.API.Auth;
using App.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace App.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(IAuthService authService, ITokenService tokenService, IOptions<TokenOptions> tokenOptions) : ControllerBase
    {
        public record LoginRequest(string Username, string Password);
        public record LoginResponse(
            string AccessToken, 
            DateTime ExpiresAtUtc, 
            string RefreshToken, 
            DateTime RefreshExpiresAtUtc,
            UserInfo User
        );
        
        public record UserInfo(
            int Id,
            string Username,
            string FullName,
            string Email,
            List<string> Roles,
            CompanyInfo? Company,
            BranchInfo? Branch
        );
        
        public record CompanyInfo(int Id, string Name, string Code);
        public record BranchInfo(int Id, string Name, int CompanyId);
        public record RegisterRequest(string Username, string FullName, string Email, string Password);
        public record RefreshRequest(string RefreshToken);
        public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
        public record AssignUserToCompanyRequest(int UserId, int CompanyId, int? BranchId);

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await authService.AuthenticateAsync(request.Username, request.Password);
            if (!result.Success)
            {
                return Unauthorized(new { error = result.Error });
            }

            var opts = tokenOptions.Value;
            var token = tokenService.CreateToken(result.User!.Id, result.User.Username, result.User.FullName, result.Roles, result.User.CompanyId, result.User.BranchId, opts, out var exp);
            var rt = await authService.IssueRefreshTokenAsync(result.User.Id);
            
            var userInfo = new UserInfo(
                result.User.Id,
                result.User.Username,
                result.User.FullName,
                result.User.Email,
                result.Roles,
                result.User.Company != null ? new CompanyInfo(result.User.Company.Id, result.User.Company.Name, result.User.Company.Code) : null,
                result.User.Branch != null ? new BranchInfo(result.User.Branch.Id, result.User.Branch.Name, result.User.Branch.CompanyId) : null
            );
            
            return Ok(new LoginResponse(token, exp, rt.RefreshToken!, rt.ExpiresAtUtc!.Value, userInfo));
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await authService.RegisterAsync(request.Username, request.FullName, request.Email, request.Password);
            if (!result.Success)
            {
                return BadRequest(new { error = result.Error });
            }
            return Ok(new { message = "Kullanıcı oluşturuldu.", userId = result.UserId });
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var result = await authService.ValidateRefreshTokenAsync(request.RefreshToken);
            if (!result.Success)
                return Unauthorized(new { error = result.Error });

            var opts = tokenOptions.Value;
            var token = tokenService.CreateToken(result.User!.Id, result.User.Username, result.User.FullName, result.Roles, result.User.CompanyId, result.User.BranchId, opts, out var exp);
            var rt = await authService.IssueRefreshTokenAsync(result.User.Id);
            
            var userInfo = new UserInfo(
                result.User.Id,
                result.User.Username,
                result.User.FullName,
                result.User.Email,
                result.Roles,
                result.User.Company != null ? new CompanyInfo(result.User.Company.Id, result.User.Company.Name, result.User.Company.Code) : null,
                result.User.Branch != null ? new BranchInfo(result.User.Branch.Id, result.User.Branch.Name, result.User.Branch.CompanyId) : null
            );
            
            return Ok(new LoginResponse(token, exp, rt.RefreshToken!, rt.ExpiresAtUtc!.Value, userInfo));
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            // userId, token claimlerinden alınır
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await authService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);
            if (!result.Success)
                return BadRequest(new { error = result.Error });
            return Ok(new { message = "Parola güncellendi." });
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
        [HttpPost("assign-company")]
        public async Task<IActionResult> AssignUserToCompany([FromBody] AssignUserToCompanyRequest request)
        {
            // TODO: UserService implement edilecek
            // var result = await userService.AssignToCompanyAsync(request.UserId, request.CompanyId, request.BranchId);
            // if (!result.Success)
            //     return BadRequest(new { error = result.Error });
            
            return Ok(new { message = "Kullanıcı şirkete atandı." });
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            // Debug için tüm claim'leri logla
            var allClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            Console.WriteLine($"Profile endpoint - All claims: {string.Join(", ", allClaims.Select(c => $"{c.Type}={c.Value}"))}");
            
            // JWT'de sub claim'i farklı şekillerde okunabilir, hepsini deneyelim
            var userIdClaim = User.Claims.FirstOrDefault(c => 
                c.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub || 
                c.Type == "sub" || 
                c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                
            if (string.IsNullOrEmpty(userIdClaim))
            {
                Console.WriteLine("Profile endpoint - Sub claim not found");
                return Unauthorized(new { error = "Sub claim not found", allClaims = allClaims });
            }
            
            if (!int.TryParse(userIdClaim, out var userId))
            {
                Console.WriteLine($"Profile endpoint - Cannot parse userId from: {userIdClaim}");
                return Unauthorized(new { error = "Invalid user ID" });
            }

            return Ok(new { 
                message = "Profile retrieved successfully", 
                userId = userId,
                allClaims = allClaims 
            });
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet("companies")]
        public async Task<IActionResult> GetAvailableCompanies()
        {
            // Debug için tüm claim'leri logla
            var allClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            Console.WriteLine($"All claims: {string.Join(", ", allClaims.Select(c => $"{c.Type}={c.Value}"))}");
            
            // JWT'de sub claim'i farklı şekillerde okunabilir, hepsini deneyelim
            var userIdClaim = User.Claims.FirstOrDefault(c => 
                c.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub || 
                c.Type == "sub" || 
                c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                
            if (string.IsNullOrEmpty(userIdClaim))
            {
                Console.WriteLine("Sub claim not found");
                return Unauthorized(new { error = "Sub claim not found" });
            }
            
            if (!int.TryParse(userIdClaim, out var userId))
            {
                Console.WriteLine($"Cannot parse userId from: {userIdClaim}");
                return Unauthorized(new { error = "Invalid user ID" });
            }

            var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();
            Console.WriteLine($"User roles: {string.Join(", ", roles)}");
            
            return Ok(new { 
                message = "Available companies retrieved successfully",
                userId = userId,
                roles = roles,
                allClaims = allClaims
            });
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "Test successful", isAuthenticated = User.Identity?.IsAuthenticated });
        }
    }
}
