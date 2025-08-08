using App.Repositories.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.UserRoles
{
    public class UserRoleRepository(AppDbContext context) : GenericRepository<UserRole>(context), IUserRoleRepository
    {
    }
}
