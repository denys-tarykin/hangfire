using System;
using Hangfire.Common;
using Hangfire.Common.Exceptions;

namespace HangfireApplication.Web.Api.Exceptions
{
    public class DtoModelIncomingValidationException : ManagedException
    {
        public DtoModelIncomingValidationException(string exceptionMessage, string errMessage)
            : base(exceptionMessage, ErrorCodes.UnexpectedRequestBody, errMessage)
        {
        }

        public DtoModelIncomingValidationException(string message, Exception exception, string errMessage)
            : base(message, exception, ErrorCodes.UnexpectedRequestBody, errMessage)
        {
        }
    }

    public class ValidationErrorDetails : IPayload
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }
}