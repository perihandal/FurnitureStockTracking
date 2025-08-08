using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Branches
{
    public interface IBranchRepository : IGenericRepository<Branch>
    {
        Task<List<Branch>> GetAllWithDetailsAsync();
   
    }
}
