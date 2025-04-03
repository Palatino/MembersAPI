using Application.Common.Interfaces;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
