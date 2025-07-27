using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.MainGroups
{
    public class MainGroupRepository(AppDbContext context) : GenericRepository<MainGroup>(context), IMainGroupRepository
    {
    }
}
