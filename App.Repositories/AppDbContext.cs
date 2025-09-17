using App.Repositories.Categories;
using App.Repositories.Companies;
using App.Repositories.StockCards;
using App.Repositories.StockTransactions;
using App.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using App.Repositories.MainGroups;
using App.Repositories.BarcodeCards;
using App.Repositories.Warehouses;
using App.Repositories.PriceDefinitions;
using App.Repositories.Branches;
using App.Repositories.SubGroups;
using App.Repositories.UserRoles;
using App.Repositories.Roles;
using App.Repositories.PriceHistories;
using App.Repositories.WarehouseStocks;
using App.Repositories.TransferRequests;

namespace App.Repositories
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<StockCard> StockCards { get; set; } = default!;
        public DbSet<Branch> Branches { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Company> Companies { get; set; } = default!;
        public DbSet<MainGroup> MainGroups { get; set; } = default!;
        public DbSet<SubGroup> SubGroups { get; set; } = default!;
        public DbSet<StockTransaction> StockTransactions { get; set; } = default!;
        public DbSet<PriceHistory> PriceHistories { get; set; } = default!;
        public DbSet<BarcodeCard> BarcodeCards { get; set; } = default!;
        public DbSet<Warehouse> Warehouses { get; set; } = default!;
        public DbSet<WarehouseStock> WarehouseStocks { get; set; } = default!;
        public DbSet<PriceDefinition> PriceDefinitions { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<UserRole> UserRoles { get; set; } = default!;
        public DbSet<Role> Roles { get; set; } = default!;
        public DbSet<TransferRequest> TransferRequests { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
