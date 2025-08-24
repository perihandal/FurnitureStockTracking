using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.StockCardServices
{
    public record class CreateStockCardResponse(int Id, string? DefaultBarcodeCode = null);
}