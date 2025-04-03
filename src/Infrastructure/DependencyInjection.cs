using Application.Common.Interfaces;
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

            return services;
        }


    }


    public class MembershipsDbContextFactory : IDesignTimeDbContextFactory<MembershipsDbContext>
    {
        public MembershipsDbContext CreateDbContext(string[] args)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            Console.WriteLine(filePath);
            Console.WriteLine(File.Exists(filePath));
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Points to the MembershipsApi directory
                .AddJsonFile("appsettings.json") // Reads appsettings.json from the MembershipsApi project
                .Build();

            // Get connection string from configuration
            var connectionString = configuration.GetConnectionString("MembershipsDbConnectionString");
            Console.WriteLine("CS:   " + connectionString);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("The connection string 'MembershipsDbConnectionString' is not set!");
            }

            // Configure DbContextOptions
            var optionsBuilder = new DbContextOptionsBuilder<MembershipsDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new MembershipsDbContext(optionsBuilder.Options);
        }
    }
}
