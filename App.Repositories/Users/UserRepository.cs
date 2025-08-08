using App.Repositories.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Users
{
    public class UserRepository(AppDbContext context) : GenericRepository<User>(context), IUserRepository
    {
    }
}
