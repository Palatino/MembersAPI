using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace Infrastructure
{

    public class MembershipsDbContextFactory : IDesignTimeDbContextFactory<MembershipsDbContext>
    {
        public MembershipsDbContext CreateDbContext(string[] args)
        {

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetValue<string>("MembershipsDbConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ConfigurationErrorsException("The connection string 'MembershipsDbConnectionString' is not set!");
            }

            var optionsBuilder = new DbContextOptionsBuilder<MembershipsDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new MembershipsDbContext(optionsBuilder.Options);
        }
    }
}
