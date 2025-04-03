using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{

    public class ErrorResponse
    {
        public int ErrorCode { get; private set; }
        public string ErrorMessage { get; private set; }

        public ErrorResponse(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
