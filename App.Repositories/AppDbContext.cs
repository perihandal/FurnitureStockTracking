using App.Repositories.Categories;
using App.Repositories.ProductionLogs;
using App.Repositories.Products;
using App.Repositories.RecipeItems;
using App.Repositories.StockTransactions;
using App.Repositories.Suppliers;
using App.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace App.Repositories
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Supplier> Suppliers { get; set; } = default!;
        public DbSet<RecipeItem> RecipeItems { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<ProductionLog> ProductionLogs { get; set; } = default!;
        public DbSet<StockTransaction> StockTransactions { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
