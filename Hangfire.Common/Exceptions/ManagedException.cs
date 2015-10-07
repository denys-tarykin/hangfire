using System;

namespace Hangfire.Common.Exceptions
{
    public class ManagedException: BaseException
    {
        public ManagedException(string exceptionMessage, ErrorCodes errorCode, String errorMessage)
            : base(exceptionMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
        public ManagedException(ErrorCodes errorCode, String errorMessage)
            : base(errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public ManagedException(string exceptionMessage, Exception exception, ErrorCodes errorCode, String errorMessage)
            : base(exceptionMessage, exception)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public ErrorCodes ErrorCode { get; private set; }
        public String ErrorMessage { get; private set; }
    }
}