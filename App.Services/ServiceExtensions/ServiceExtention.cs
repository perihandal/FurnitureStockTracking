using App.Repositories.Categories;
using App.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using App.Services.StockCardServices;
using App.Services.CategoryServices;
using App.Services.CompanyServices;
using App.Services.WareHouseServices;
using App.Services.BranchServices;
using App.Services.MainGroupServices;
using App.Services.SubGroupServices;
using App.Services.PriceDefinitionServices;

namespace App.Services.ServiceExtensions
{
    public static class ServiceExtention
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IStockCardService, StockCardService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IWareHouseService, WareHouseService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IMainGroupService, MainGroupService>();
            services.AddScoped<ISubGroupService, SubGroupService>();
            services.AddScoped<IPriceDefinitionService, PriceDefinitionService>();

            return services;
        }
    }
}
