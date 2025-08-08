using App.Repositories.StockCards;
using System;

namespace App.Services.StockCardServices
{
    public record class CreateStockCardRequest(
        string Name,
        string Code,
        StockCardType Type,
        string Unit,
        decimal Tax,
        int CompanyId,
        int UserId,
        int BranchId,
        int MainGroupId,
        int? SubGroupId,
        int? CategoryId
    );
}
