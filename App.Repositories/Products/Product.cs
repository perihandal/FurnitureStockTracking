using App.Repositories.Categories;
using App.Repositories.ProductionLogs;
using App.Repositories.RecipeItems;
using App.Repositories.Suppliers;
using App.Repositories.StockTransactions;

namespace App.Repositories.Products
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
        public string Unit { get; set; } = default!;
        public decimal Price { get; set; }
        public decimal StockQuantity { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public ICollection<RecipeItem> RecipeItems { get; set; } = new List<RecipeItem>();
        public ICollection<RecipeItem> UsedInRecipes { get; set; } = new List<RecipeItem>();
        public ICollection<ProductionLog> ProductionLogs { get; set; } = new List<ProductionLog>();
        public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
    }
}
