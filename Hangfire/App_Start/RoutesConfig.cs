using System.Web.Http;

namespace HangfireApplication.Web.Api
{
    public class RoutesConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("PingServices", "service/ping",
                new { controller = "Service", action = "Ping" });
            /*config.Routes.MapHttpRoute("Calculation", "calculation/calc",
                new { controller = "Calculation", action = "Calculate" });*/
        } 
    }
}