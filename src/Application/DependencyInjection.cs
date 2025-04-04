using Application.Common.Interfaces;
using Application.Logging;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Application
{
    [ExcludeFromCodeCoverage]

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
            services.AddScoped(typeof(IMembershipsService), typeof(MembershipsService));
            return services;


        }

    }
}
