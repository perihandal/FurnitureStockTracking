using App.Repositories.Categories;
using App.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Services.StockCardServices;

namespace App.Services.ServiceExtensions
{
    public static class ServiceExtention
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IStockCardService, StockCardService>();

            // services.AddScoped<ICategoryService, CategoryService>();
            // services.AddScoped<IProductionLogService, ProductionLogService>();
            // services.AddScoped<IRecipeItemService, RecipeItemService>();
            // services.AddScoped<IStockTransactionService, StockTransactionService>();
            // services.AddScoped<ISupplierService, SupplierService>();
            return services;
        }
    }
}
