using App.Repositories.SubGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.SubGroups
{
    public class SubGroupRepository(AppDbContext context) : GenericRepository<SubGroup>(context),ISubGroupRepository
    {
    }
}
