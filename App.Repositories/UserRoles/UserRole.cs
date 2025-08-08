using App.Repositories.Users;
using App.Repositories.Roles;
using System.Data;

namespace App.Repositories.UserRoles
{
    public class UserRole
    {
        public int? UserId { get; set; }
        public User? User { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; } = default!;
    }
}
