using Application.Logging;
using Microsoft.AspNetCore.Mvc;

namespace MembershipsApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MembershipsController : ControllerBase
    {
        private readonly ILoggerAdapter<PingController> _logger;
        public MembershipsController(ILoggerAdapter<PingController> logger)
        {
            _logger = logger;
        }
    }
}
