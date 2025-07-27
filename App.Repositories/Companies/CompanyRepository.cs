using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Companies
{
    public class CompanyRepository(AppDbContext context) : GenericRepository<Company>(context), ICompanyRepository
    {
    }
}
