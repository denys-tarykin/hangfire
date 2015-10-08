using System.Web.Http;
using Hangfire;
using Hangfire.Server;
using Microsoft.Practices.Unity;

namespace Hangfire.Web.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireStorageDbContext");
        }
    }
}
