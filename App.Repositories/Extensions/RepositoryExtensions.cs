using App.Repositories.Warehauses;
using App.Repositories.Categories;
using App.Repositories.Companies;
using App.Repositories.StockTransactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Repositories.MainGroups;
using App.Repositories.StockCards;
using App.Repositories.Branches;
using App.Repositories.PriceDefinitions;
using App.Repositories.BarcodeCards;
using App.Repositories.SubGroups;

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
            services.AddScoped<IStockCardRepository, StockCardRepository>();
            services.AddScoped<ICategoryRepository,CategoryRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IMainGroupRepository, MainGroupRepository>();
            services.AddScoped<ISubGroupRepository, SubGroupRepository>();
            services.AddScoped<IPriceDefinitionRepository, PriceDefinitionRepository>();
            services.AddScoped<IStockTransactionRepository, StockTransactionRepository>();
            services.AddScoped<IBarcodeCardRepository, BarcodeCardRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof (IGenericRepository<>), typeof (GenericRepository<>));
            return services;
        }
    }
}
