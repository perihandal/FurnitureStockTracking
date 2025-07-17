﻿using Microsoft.EntityFrameworkCore;
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

            return services;
        }
    }
}
