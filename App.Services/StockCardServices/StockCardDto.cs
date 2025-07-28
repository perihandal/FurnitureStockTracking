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
        string Name
        //string Code,
        //StockCardType Type,
        //string Unit,
        //decimal Tax,
        //DateTime CreatedDate,
        //string CompanyName,
        //string BranchName,
        //string MainGroupName,
        //string? SubGroupName,
        //string? CategoryName
    );
}
