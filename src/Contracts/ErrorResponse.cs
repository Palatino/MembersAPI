using Swashbuckle.AspNetCore.Annotations;

namespace Contracts
{

    public class ErrorResponse
    {
        [SwaggerSchema("Http error code")]
        public int ErrorCode { get; private set; }
        [SwaggerSchema("Error description")]
        public string ErrorMessage { get; private set; }

        public ErrorResponse(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
