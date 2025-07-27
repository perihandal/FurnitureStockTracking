using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Branches
{
    public class BranchRepository(AppDbContext context) : GenericRepository<Branch>(context), IBranchRepository
    {
    }
}
