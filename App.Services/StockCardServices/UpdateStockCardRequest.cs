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
    string Unit,
    decimal Price,
    decimal StockQuantity,
    bool IsActive,
    DateTime CreatedDate
);

}
