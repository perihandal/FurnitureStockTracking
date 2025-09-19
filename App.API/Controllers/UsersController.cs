using App.Repositories.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace App.API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController(IUserRepository userRepository, IUnitOfWork unitOfWork) : ControllerBase
    {
        public record AssignCompanyRequest(int CompanyId, int? BranchId);
        public record CreateUserRequest(string Username, string FullName, string Email, string Password, int? CompanyId, int? BranchId);
        public record UpdateUserRequest(string FullName, string Email, bool IsActive, int? CompanyId, int? BranchId, List<string> Roles);

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await userRepository.GetAll()
                .Include(u => u.Company)
                .Include(u => u.Branch)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Select(u => new { 
                    u.Id, 
                    u.Username, 
                    u.FullName, 
                    u.Email, 
                    u.IsActive,
                    Company = u.Company != null ? new { u.Company.Id, u.Company.Name, u.Company.Code } : null,
                    Branch = u.Branch != null ? new { u.Branch.Id, u.Branch.Name } : null,
                    Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
                }).ToListAsync();
            return Ok(users);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest request)
        {
            var user = await userRepository.GetAll()
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == id);
                
            if (user == null)
                return NotFound();

            // Kullanıcı bilgilerini güncelle
            user.FullName = request.FullName;
            user.Email = request.Email;
            user.IsActive = request.IsActive;
            user.CompanyId = request.CompanyId;
            user.BranchId = request.BranchId;

            userRepository.Update(user);
            await unitOfWork.SaveChangesAsync();

            return Ok(new { message = "Kullanıcı başarıyla güncellendi." });
        }

        [HttpPut("{id}/toggle-status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            user.IsActive = !user.IsActive;
            userRepository.Update(user);
            await unitOfWork.SaveChangesAsync();

            return Ok(new { 
                message = $"Kullanıcı {(user.IsActive ? "aktif" : "pasif")} duruma getirildi.",
                isActive = user.IsActive 
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await userRepository.GetAll()
                .Include(u => u.Company)
                .Include(u => u.Branch)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
                
            if (user == null)
                return NotFound();

            return Ok(new { 
                user.Id, 
                user.Username, 
                user.FullName, 
                user.Email, 
                user.IsActive,
                Company = user.Company != null ? new { user.Company.Id, user.Company.Name, user.Company.Code } : null,
                Branch = user.Branch != null ? new { user.Branch.Id, user.Branch.Name } : null,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            });
        }

        [HttpPut("{id}/assign-company")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignCompany(int id, [FromBody] AssignCompanyRequest request)
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            user.CompanyId = request.CompanyId;
            user.BranchId = request.BranchId;

            userRepository.Update(user);
            await unitOfWork.SaveChangesAsync();

            return Ok(new { message = "Kullanıcı şirkete atandı." });
        }

        [HttpPut("{id}/remove-company")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveFromCompany(int id)
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            user.CompanyId = null;
            user.BranchId = null;

            userRepository.Update(user);
            await unitOfWork.SaveChangesAsync();

            return Ok(new { message = "Kullanıcı şirketten çıkarıldı." });
        }

        [HttpGet("unassigned")]
        public async Task<IActionResult> GetUnassignedUsers()
        {
            var users = await userRepository.GetAll()
                .Where(u => u.CompanyId == null && u.IsActive)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Select(u => new { 
                    u.Id, 
                    u.Username, 
                    u.FullName, 
                    u.Email,
                    Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
                }).ToListAsync();
            return Ok(users);
        }

        [HttpGet("profile")]
        [Authorize] // Sadece giriş yapmış olması yeterli, rol kontrolü yok
        public async Task<IActionResult> GetMyProfile()
        {
            // JWT token'dan kullanıcı ID'sini al
            Console.WriteLine($"Profile endpoint - All claims: {string.Join(", ", User.Claims.Select(c => $"{c.Type}={c.Value}"))}");
            
            var userIdClaim = User.FindFirst("userId")?.Value ?? 
                             User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ??
                             User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            Console.WriteLine($"UserIdClaim: {userIdClaim}");
            
            if (!int.TryParse(userIdClaim, out int userId))
            {
                Console.WriteLine("Failed to parse userId from token");
                return Unauthorized();
            }

            var user = await userRepository.GetAll()
                .Include(u => u.Company)
                .Include(u => u.Branch)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
                
            if (user == null)
                return NotFound();

            return Ok(new { 
                user.Id, 
                user.Username, 
                user.FullName, 
                user.Email, 
                user.IsActive,
                Company = user.Company != null ? new { user.Company.Id, user.Company.Name, user.Company.Code } : null,
                Branch = user.Branch != null ? new { user.Branch.Id, user.Branch.Name } : null,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            });
        }

        [HttpGet("my-status")]
        [Authorize] // Sadece giriş yapmış olması yeterli
        public async Task<IActionResult> GetMyStatus()
        {
            Console.WriteLine($"My-status endpoint - All claims: {string.Join(", ", User.Claims.Select(c => $"{c.Type}={c.Value}"))}");
            
            var userIdClaim = User.FindFirst("userId")?.Value ?? 
                             User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ??
                             User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            Console.WriteLine($"UserIdClaim: {userIdClaim}");
            
            if (!int.TryParse(userIdClaim, out int userId))
            {
                Console.WriteLine("Failed to parse userId from token in my-status");
                return Unauthorized();
            }

            var user = await userRepository.GetAll()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
                
            if (user == null)
                return NotFound();

            return Ok(new { 
                userId = user.Id,
                isActive = user.IsActive,
                roles = user.UserRoles.Select(ur => ur.Role.Name).ToList(),
                hasAdminRole = user.UserRoles.Any(ur => ur.Role.Name == "Admin"),
                hasEditorRole = user.UserRoles.Any(ur => ur.Role.Name == "Editor"),
                hasUserRole = user.UserRoles.Any(ur => ur.Role.Name == "User")
            });
        }
    }
}
