using Swashbuckle.AspNetCore.Annotations;

namespace Contracts
{

    /// <summary>
    /// Response type for any unsuccesful operation
    /// </summary>
    public class ErrorResponse
    {

        /// <summary>
        /// Http error code
        /// </summary>
        /// <example>404</example>
        public int ErrorCode { get; private set; }


        /// <summary>
        /// Error description
        /// </summary>
        /// <example>Resource not found</example>
        public string ErrorMessage { get; private set; }

        public ErrorResponse(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
