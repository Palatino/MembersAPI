using Application.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MembershipsApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PingController : ControllerBase
    {
        private readonly ILoggerAdapter<PingController> _logger;
        public PingController(ILoggerAdapter<PingController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(string))]
        public IActionResult Ping()
        {
            _logger.LogInformation("Ping requested");
            return Ok($"Hello there, it's {DateTime.UtcNow} right now");
        }

    }
}
