using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Common.Logging;
using Hangfire.Web.Api.DtoModel.Common;
using Hangfire.Web.Api.Exceptions;

namespace Hangfire.Web.Api.RequestFilter
{
    public class ValidationActionFilter : ActionFilterAttribute
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly bool _enableVerboseMode =
            Convert.ToBoolean(ConfigurationManager.AppSettings["EnableVerboseMode"]);

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => new ValidationErrorDetails()
                    {
                        PropertyName = e.Key,
                        ErrorMessage =
                            (e.Value.Errors.First().Exception != null) &&
                            String.IsNullOrEmpty(e.Value.Errors.First().ErrorMessage)
                                ? e.Value.Errors.First().Exception.Message
                                : e.Value.Errors.First().ErrorMessage
                    }).ToArray();
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                var responseData =
                    new DtoModelOutgoing(
                        new DtoModelIncomingValidationException("Validation failed", "Validation failed"),
                        _enableVerboseMode ? errors : null);
                response.Content = new ObjectContent<DtoModelOutgoing>(responseData, new JsonMediaTypeFormatter());
                // TODO: Add logger!
                actionContext.Response = response;
            }
        }
    }
}