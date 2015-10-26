using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hangfire.Logging;
using Hangfire.Services.Api;
using log4net;
using Hangfire.SqlServer;
using Microsoft.Practices.Unity;
using NLog;

namespace Hangfire.Server
{
    class Program
    {
        private static IUnityContainer unityContainer;
        //private static Logger log;
        private static Logger logger ;
        static void Main()
        {
           // log4net.Config.XmlConfigurator.Configure();00000000
            logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("Worker Host process has been started");
            unityContainer = UnityConfig.Register();
            UnityConfig.ConfigureDefaults(unityContainer);
            var storage = new SqlServerStorage("HangfireStorageDbContext");
            var options = new BackgroundJobServerOptions()
            {
                WorkerCount = 10
            }; 
            
            UnityJobActivator unityJobActivator = new UnityJobActivator(unityContainer);
            GlobalConfiguration.Configuration.UseActivator(unityJobActivator);
            JobActivator.Current = unityJobActivator;

            GlobalJobFilters.Filters.Add(new ChildContainerPerJobFilterAttribute(unityJobActivator));
            var server = new BackgroundJobServer(options, storage);
            server.Start();
            Thread.Sleep(300000);
            server.Stop();
        }

    }
}
