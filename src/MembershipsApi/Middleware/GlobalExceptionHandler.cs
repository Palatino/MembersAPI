using Application.Logging;
using Contracts;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;

namespace MembershipsApi.Middleware
{

    //Global exception handler to ensure any unexpected error is captured in the logs
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILoggerAdapter<GlobalExceptionHandler> _logger;


        public GlobalExceptionHandler(ILoggerAdapter<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception, "Unhandled exception occurred: {Message}", exception.Message);

            var response = new ErrorResponse(500, "Server Error");

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = 500;
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response), cancellationToken);
            return true;
        }
    }
}
