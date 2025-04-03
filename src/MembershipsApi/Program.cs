
using Application;
using Infrastructure;
using Infrastructure.Utils;
using MembershipsApi.Middleware;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace MembershipsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //Set application insights if not running locally
            if (!builder.Environment.IsEnvironment("Local"))
            {
                builder.Services.AddApplicationInsightsTelemetry(options =>
                {
                    options.ConnectionString = builder.Configuration.GetValue<string>("ApplicationInsightsWebConnectionString");
                });

                builder.Logging.AddApplicationInsights(
                    configureTelemetryConfiguration: (config) =>
                        config.ConnectionString = builder.Configuration.GetValue<string>("ApplicationInsightsWebConnectionString"),
                        configureApplicationInsightsLoggerOptions: (options) => { }
                );
            }


            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy(), false));
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            builder.Services.AddRouting(options => options.LowercaseUrls = true);

            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddEndpointsApiExplorer();

            //Add swagger
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Memberships API",
                    Description = "Example API to manage membership",
                    Contact = new OpenApiContact
                    {
                        Name = "Pablo Alvarez",
                        Email = "pablo.alvarez@arcadis.com"
                    }
                });
                options.EnableAnnotations();
            });
            builder.Services.AddSwaggerGenNewtonsoftSupport();

            //Add healthceck, must be configured in Azure accordingly
            builder.Services.AddHealthChecks();


            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHsts();
            app.UseHttpsRedirection();
            app.MapControllers();

            //Automatically apply migrations to DB
            //Not the best solution, better to do this using some CI/CD pipeline
            //Done this way for simplicity
            using (var serviceScope = app.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var dbUpdater = services.GetRequiredService<DbUpdater>();
                if (dbUpdater is not null)
                {
                    dbUpdater.UpdateDB();
                }
            }

            //Add endpoint for azure health check 
            app.MapHealthChecks("/healthcheck");
            app.Run();
        }
    }
}
