using System;
using System.Configuration;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http;
using Common.Logging;
using Hangfire.Dao.EntityFrameworkImpl.Session;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HangfireApplication.Web.Api
{
    public static class WebApiConfig
    {
        private static IUnityContainer unityContainer;
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Register(HttpConfiguration config)
        { 
            // Ioc Container configuration
            unityContainer = UnityConfig.Register(config);
            // Web API configuration
            ConfigureDefaults(config);
            config.MapHttpAttributeRoutes();
           
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

            try
            {
                var dbModelHolder = unityContainer.Resolve<DbModelHolder>();
                dbModelHolder.ConnectionString =
                    ConfigurationManager.ConnectionStrings["CommonDbContext"].ConnectionString;
            }
            catch (Exception e)
            {
                _logger.Error("dbModelHanlder Configuration failed", e);
            }
        }
    }
}
