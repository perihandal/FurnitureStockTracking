using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.CompanyServices
{
    public record class CreateCompanyRequest(
        string Code,
        string Name,
        string TaxNumber,
        string Address,
        string Phone,
        int UserId,
        bool IsActive
    );
}

