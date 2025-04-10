
using Application;
using Contracts;
using Infrastructure;
using Infrastructure.Utils;
using MembershipsApi.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MembershipsApi
{
    [ExcludeFromCodeCoverage]
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


            //Map bad request error response to custom ErrorResponse class for consistency
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToList();

                    var errorMessage = string.Join(", ", errors);
                    var response = new ErrorResponse(400, errorMessage);
                    return new BadRequestObjectResult(response);
                };
            });

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
                        Email = "p.alvarez.rio@gmail.com"
                    }
                });
                options.EnableAnnotations();

                var mainAssemblyXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var mainAssemblyXmlPath = Path.Combine(AppContext.BaseDirectory, mainAssemblyXmlFile);
                options.IncludeXmlComments(mainAssemblyXmlPath);

                // Include XML comments for the Contracts assembly
                var contractsAssemblyXmlFile = "Contracts.xml";
                var contractsAssemblyXmlPath = Path.Combine(AppContext.BaseDirectory, contractsAssemblyXmlFile);
                options.IncludeXmlComments(contractsAssemblyXmlPath);
            });
            builder.Services.AddSwaggerGenNewtonsoftSupport();

            //Add HealthCheck, must be configured in Azure accordingly
            builder.Services.AddHealthChecks();


            var app = builder.Build();

            app.UseSwagger();
            app.UseExceptionHandler("/Error");

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
