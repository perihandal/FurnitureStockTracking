using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Companies
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        Task<List<Company>> GetAllWithDetailsAsync();
    }
}
