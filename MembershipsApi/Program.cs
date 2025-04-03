
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;

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

            //Add endpoint for azure health check 
            app.MapHealthChecks("/healthcheck");
            app.Run();
        }
    }
}
