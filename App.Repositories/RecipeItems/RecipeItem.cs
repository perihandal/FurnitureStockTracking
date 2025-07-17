using App.Repositories.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.RecipeItems
{
    public class RecipeItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public int RequiredProductId { get; set; }
        public Product RequiredProduct { get; set; } = default!;

        public int Quantity { get; set; }
    }
}
