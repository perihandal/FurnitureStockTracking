using App.Repositories.MainGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.SubGroups
{
    public interface ISubGroupRepository: IGenericRepository<SubGroup>
    {
        Task<List<SubGroup>> GetAllWithDetailsAsync();
    }
}
