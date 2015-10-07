using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hangfire.Web.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration
            ConfigureDefaults(config);
            config.MapHttpAttributeRoutes();
            // Ioc Container configuration
            UnityConfig.Register(config);
            // Web API routes                        
            RoutesConfig.Register(config);  
        }

        private static void ConfigureDefaults(HttpConfiguration config)
        {
            //config.Filters.Add(new ExceptionActionFilter());
            //config.Filters.Add(new ValidationActionFilter());
            //Delete all formatter and add only JSON formatting for request and response
            config.Formatters.Clear();
            var formatter = new JsonMediaTypeFormatter()
            {
                SerializerSettings =
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                },
            };
            config.Formatters.Add(formatter);
        }
    }
}
