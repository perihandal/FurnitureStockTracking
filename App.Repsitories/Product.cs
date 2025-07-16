using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories
{
    public enum ProductType
    {
        NihaiUrun,
        AraUrun,
        Hammadde
    }
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public ProductType Type { get; set; }

        public string Unit { get; set; }=default!;// adet, kg, metre vb.

        public decimal Price { get; set; }

        public decimal StockQuantity { get; set; } // anlık stok

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
