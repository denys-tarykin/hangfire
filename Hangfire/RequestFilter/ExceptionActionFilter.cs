using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http.Filters;
using Common.Logging;
using Hangfire.Common;
using Hangfire.Common.Exceptions;
using Hangfire.Web.Api.DtoModel.Common;
using Hangfire.Web.Api.Exceptions;

namespace Hangfire.Web.Api.RequestFilter
{
    public class ExceptionActionFilter : ExceptionFilterAttribute
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly bool _enableVerboseMode =
            Convert.ToBoolean(ConfigurationManager.AppSettings["EnableVerboseMode"]);

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Exception exception = actionExecutedContext.Exception;
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            DtoModelOutgoing responseData = null;
            responseData = new DtoModelOutgoing(new ManagedException(ErrorCodes.InternalServerError, "Internal Server error"), _enableVerboseMode ? new ValidationErrorDetails() { ErrorMessage = exception.Message } : null);
            response.Content = new ObjectContent<DtoModelOutgoing>(responseData, new JsonMediaTypeFormatter());
            //general HTTP 500 response
            _logger.Error("Unhandled exception occurs.", exception);
            actionExecutedContext.Response = response;
        }

    }
}