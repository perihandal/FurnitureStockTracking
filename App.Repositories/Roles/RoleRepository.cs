using App.Repositories.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Roles
{
    public class RoleRepository(AppDbContext context) : GenericRepository<Role>(context), IRoleRepository
    {
    }
}
