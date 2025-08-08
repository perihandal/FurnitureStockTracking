using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.MainGroups
{
    public interface IMainGroupRepository : IGenericRepository<MainGroup>
    {
        Task<List<MainGroup>> GetAllWithDetailsAsync();
    }
}
