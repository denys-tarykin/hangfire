using System;
using System.Web.Http;
using Hangfire;
using Hangfire.Server;
using Hangfire.SqlServer;
using Microsoft.Practices.Unity;
using GlobalConfiguration = Hangfire.GlobalConfiguration;

namespace HangfireApplication.Web.Api
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
