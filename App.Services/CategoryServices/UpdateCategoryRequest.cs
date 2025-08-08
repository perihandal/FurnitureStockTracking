using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.CategoryServices
{
    public record class UpdateCategoryRequest(
    string Code,
    string Name,
    bool IsActive,
    int UserId,
    DateTime CreateDate,
    int BranchId,
    int CompanyId);
 
}
