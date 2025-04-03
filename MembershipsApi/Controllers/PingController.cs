using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MembershipsApi.Controllers
{
    [ApiController]
    [Authorize()]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {
        private readonly ILogger<PingController> _logger;
        public PingController(ILogger<PingController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(string))]
        public IActionResult Ping()
        {
            _logger.LogInformation("Ping requested");
            return Ok($"Hello DEAS user at {DateTime.UtcNow}    ");
        }

    }
}
