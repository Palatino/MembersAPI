using Application.Common.Interfaces;
using Application.Logging;
using Contracts;
using Contracts.Enums;
using ErrorOr;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MembershipsApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MembershipsController : ControllerBase
    {
        private readonly ILoggerAdapter<PingController> _logger;
        private readonly IMembershipsService _membershipsService;
        public MembershipsController(ILoggerAdapter<PingController> logger, IMembershipsService membershipsService)
        {
            _logger = logger;
            _membershipsService = membershipsService;
        }

        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(MembershipDto), contentTypes: ["application/json"])]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(ErrorResponse), contentTypes: ["application/json"])]
        public async Task<IActionResult> GetMemberships(CountryDto? country = null, SubscriptionTypeDto? subscriptionType = null)
        {
            _logger.LogInformation($"Function {nameof(GetMemberships)} received a request");

            var result = await _membershipsService.GetMemberships(country, subscriptionType);
            if (!result.IsError)
            {
                _logger.LogInformation($"Function {nameof(GetMemberships)} returned 200 code");
                return new OkObjectResult(result.Value);
            }
            _logger.LogInformation($"Function {nameof(GetMemberships)} returned 500 code");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(500, result.FirstError.Description));
        }

        [HttpGet("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(MembershipDto[]), contentTypes: ["application/json"])]
        [SwaggerResponse(StatusCodes.Status404NotFound, type: typeof(ErrorResponse), contentTypes: ["application/json"])]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(ErrorResponse), contentTypes: ["application/json"])]
        public async Task<IActionResult> GetMembership(Guid id)
        {
            _logger.LogInformation($"Function {nameof(GetMembership)} received a request");

            var result = await _membershipsService.GetMembership(id);
            if (!result.IsError)
            {
                _logger.LogInformation($"Function {nameof(GetMembership)} returned 200 code");
                return new OkObjectResult(result.Value);
            }
            var error = result.FirstError;
            if (error.Type == ErrorType.NotFound)
            {
                _logger.LogInformation($"Function {nameof(GetMembership)} returned 404 code");
                return new NotFoundObjectResult(new ErrorResponse(404, error.Description));
            }
            _logger.LogInformation($"Function {nameof(GetMembership)} returned 500 code");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(500, result.FirstError.Description));
        }


        [HttpDelete("{id}")]
        [SwaggerResponse(StatusCodes.Status204NoContent, contentTypes: ["application/json"])]
        [SwaggerResponse(StatusCodes.Status404NotFound, type: typeof(ErrorResponse), contentTypes: ["application/json"])]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(ErrorResponse), contentTypes: ["application/json"])]
        public async Task<IActionResult> DeleteMembership(Guid id)
        {
            _logger.LogInformation($"Function {nameof(DeleteMembership)} received a request");

            var result = await _membershipsService.DeleteMembershipById(id);

            if (!result.IsError)
            {
                _logger.LogInformation($"Function {nameof(DeleteMembership)} returned 204 code");
                return NoContent();
            }
            var error = result.FirstError;
            if (error.Type == ErrorType.NotFound)
            {
                _logger.LogInformation($"Function {nameof(DeleteMembership)} returned 404 code");
                return new NotFoundObjectResult(new ErrorResponse(404, error.Description));
            }
            _logger.LogInformation($"Function {nameof(DeleteMembership)} returned 500 code");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(500, error.Description));

        }

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, type: typeof(MembershipDto), contentTypes: ["application/json"])]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(ErrorResponse), contentTypes: ["application/json"])]
        public async Task<IActionResult> CreateMembership([FromBody] MembershipDto membership)
        {
            _logger.LogInformation($"Function {nameof(CreateMembership)} received a request");

            var result = await _membershipsService.CreateNewMembership(membership);

            if (!result.IsError)
            {
                _logger.LogInformation($"Function {nameof(CreateMembership)} returned 201 code");
                return CreatedAtAction(nameof(CreateMembership), result.Value);

            }
            _logger.LogInformation($"Function {nameof(CreateMembership)} returned 500 code");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(500, result.FirstError.Description));
        }

        [HttpPut("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(MembershipDto), contentTypes: ["application/json"])]
        [SwaggerResponse(StatusCodes.Status404NotFound, type: typeof(ErrorResponse), contentTypes: ["application/json"])]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(ErrorResponse), contentTypes: ["application/json"])]
        public async Task<IActionResult> UpdateMembership(Guid id, [FromBody] MembershipDto membershipDto)
        {
            _logger.LogInformation($"Function {nameof(UpdateMembership)} received a request");

            var result = await _membershipsService.UpdateMembership(id, membershipDto);

            if (!result.IsError)
            {
                _logger.LogInformation($"Function {nameof(UpdateMembership)} returned 200 code");
                return new OkObjectResult(result.Value);
            }
            var error = result.FirstError;
            if (error.Type == ErrorType.NotFound)
            {
                _logger.LogInformation($"Function {nameof(UpdateMembership)} returned 404 code");
                return new NotFoundObjectResult(new ErrorResponse(404, error.Description));
            }
            if (error.Type == ErrorType.Validation)
            {
                _logger.LogInformation($"Function {nameof(UpdateMembership)} returned 400 code");
                return new BadRequestObjectResult(new ErrorResponse(400, error.Description));
            }
            _logger.LogInformation($"Function {nameof(UpdateMembership)} returned 500 code");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(500, error.Description));
        }
    }
}
