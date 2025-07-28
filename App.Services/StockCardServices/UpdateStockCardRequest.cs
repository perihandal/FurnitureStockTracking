using App.Repositories.StockCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.StockCardServices
{
    public record class UpdateStockCardRequest(
        string Name,
        string Code,
        StockCardType Type,
        string Unit,
        decimal Tax,
        bool IsActive,
        int CompanyId,
        int BranchId,
        int MainGroupId,
        int? SubGroupId,
        int? CategoryId
);

}
