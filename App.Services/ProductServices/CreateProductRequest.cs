using App.Repositories.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.ProductServices
{
    public record class CreateProductRequest(string Name,
        ProductType Type,
        string Unit,
        decimal Price,
        decimal StockQuantity,
        int? CategoryId,
        int? SupplierId);

}
