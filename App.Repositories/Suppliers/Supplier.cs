using App.Repositories.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Suppliers
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Phone { get; set; } =default!;
        public string? Email { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}

