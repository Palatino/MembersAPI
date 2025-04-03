using Application.Common.Interfaces;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            var dbConnectionString = configuration.GetValue<string>("MembershipsDbConnectionString");
            if (dbConnectionString is null)
            {
                throw new ConfigurationErrorsException("Environment variable MembershipsDbConnectionString is not set!");
            }
            services.AddDbContext<MembershipsDbContext>(options =>
            {
                options.UseSqlServer(dbConnectionString);
            });

            services.AddScoped<IMembershipsDbContext, MembershipsDbContext>();
            services.AddScoped<DbUpdater, DbUpdater>();

            return services;
        }


    }


}
