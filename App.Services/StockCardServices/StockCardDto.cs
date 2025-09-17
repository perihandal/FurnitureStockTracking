using App.Repositories.StockCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace App.Services.StockCardServices
{
    public record class StockCardDto(
        int Id,
        string Name,
        string Code,
        StockCardType Type,
        string Unit,
        decimal Tax,
        DateTime CreatedDate,
        int CompanyId,
        string CompanyName,
        int BranchId,
        string BranchName,
        int MainGroupId,
        string MainGroupName,
        int SubGroupId,
        string? SubGroupName,
        int CategoryId,
        string? CategoryName,
        List<string> barcodes
    );
}
