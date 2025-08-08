using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.WareHouseServices
{
    public record class CreateWareHouseRequest(
        string Code,                
        string Name,                
        string Address,             
        string Phone,               
        bool IsActive,             
        int CompanyId,              
        int BranchId ,
        int UserId

    );
}

