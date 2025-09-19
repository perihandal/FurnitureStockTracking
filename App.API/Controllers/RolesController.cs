using App.Repositories.Roles;
using App.Repositories.UserRoles;
using App.Repositories.Users;
using App.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.API.Controllers
{
    [ApiController]
    [Route("roles")]
    [Authorize(Roles = "Admin")]
    public class RolesController(IUserRepository userRepository, IRoleRepository roleRepository, IGenericRepository<UserRole> userRoleRepository, IUnitOfWork unitOfWork) : ControllerBase
    {
        public record AssignRoleRequest(int UserId, string RoleName);
        public record UpdateUserRolesRequest(int UserId, List<string> RoleNames);

        [HttpPost("assign")]
        public async Task<IActionResult> Assign([FromBody] AssignRoleRequest request)
        {
            var user = await userRepository.GetByIdAsync(request.UserId);
            if (user is null) return NotFound(new { error = "Kullanıcı bulunamadı." });

            var role = await roleRepository.Where(r => r.Name == request.RoleName).FirstOrDefaultAsync();
            if (role is null) return NotFound(new { error = "Rol bulunamadı." });

            var exists = await userRoleRepository.Where(ur => ur.UserId == request.UserId && ur.RoleId == role.Id).AnyAsync();
            if (!exists)
            {
                await userRoleRepository.AddAsync(new UserRole { UserId = request.UserId, RoleId = role.Id });
                await unitOfWork.SaveChangesAsync();
            }
            return Ok(new { message = "Rol atandı." });
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await roleRepository.GetAll().Select(r => r.Name).ToListAsync();
            return Ok(roles);
        }

        [HttpPut("{userId}/roles")]
        public async Task<IActionResult> UpdateUserRoles(int userId, [FromBody] UpdateUserRolesRequest request)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null) return NotFound(new { error = "Kullanıcı bulunamadı." });

            // Mevcut rolleri sil
            var existingUserRoles = await userRoleRepository.Where(ur => ur.UserId == userId).ToListAsync();
            foreach (var userRole in existingUserRoles)
            {
                userRoleRepository.Delete(userRole);
            }

            // Yeni rolleri ekle
            foreach (var roleName in request.RoleNames)
            {
                var role = await roleRepository.Where(r => r.Name == roleName).FirstOrDefaultAsync();
                if (role != null)
                {
                    await userRoleRepository.AddAsync(new UserRole { UserId = userId, RoleId = role.Id });
                }
            }

            await unitOfWork.SaveChangesAsync();
            return Ok(new { message = "Kullanıcı rolleri güncellendi." });
        }

        [HttpDelete("{userId}/roles/{roleName}")]
        public async Task<IActionResult> RemoveRoleFromUser(int userId, string roleName)
        {
            var role = await roleRepository.Where(r => r.Name == roleName).FirstOrDefaultAsync();
            if (role == null) return NotFound(new { error = "Rol bulunamadı." });

            var userRole = await userRoleRepository.Where(ur => ur.UserId == userId && ur.RoleId == role.Id).FirstOrDefaultAsync();
            if (userRole == null) return NotFound(new { error = "Kullanıcının bu rolü yok." });

            userRoleRepository.Delete(userRole);
            await unitOfWork.SaveChangesAsync();
            return Ok(new { message = "Rol kullanıcıdan kaldırıldı." });
        }
    }
}
