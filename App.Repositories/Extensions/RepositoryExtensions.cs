using App.Repositories.Categories;
using App.Repositories.ProductionLogs;
using App.Repositories.Products;
using App.Repositories.RecipeItems;
using App.Repositories.StockTransactions;
using App.Repositories.Suppliers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = configuration.GetSection
                (ConnectionStringOption.Key).Get<ConnectionStringOption>();

            options.UseMySql(
                    connectionString!.MySqlConnection,
                    ServerVersion.AutoDetect(connectionString.MySqlConnection),
                    mySqlOptionsAction =>
                    {
                        mySqlOptionsAction.MigrationsAssembly(typeof(RepositoryAssembly).Assembly.FullName);
                    });

            });
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository,CategoryRepository>();
            services.AddScoped<IProductionLogRepository, ProductionLogRepository>();
            services.AddScoped<IRecipeItemRepository, RecipeItemRepository>();
            services.AddScoped<IStockTransactionRepository, StockTransactionRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped(typeof (IGenericRepository<>), typeof (GenericRepository<>));
            return services;
        }
    }
}
