using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
           // log4net.Config.XmlConfigurator.Configure();
            logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("Worker Host process has been started");
            unityContainer = UnityConfig.Register();
            var storage = new SqlServerStorage("HangfireStorageDbContext");
            var options = new BackgroundJobServerOptions()
            {
                WorkerCount = 40
            }; 
            
            UnityJobActivator unityJobActivator = new UnityJobActivator(unityContainer);
            GlobalConfiguration.Configuration.UseActivator(unityJobActivator);
            JobActivator.Current = unityJobActivator;

            GlobalJobFilters.Filters.Add(new ChildContainerPerJobFilterAttribute(unityJobActivator));
            var start = GetTimeStamp(DateTime.Now) + TimeSpan.FromMinutes(6).TotalMilliseconds;
            var server = new BackgroundJobServer(options, storage);
            while (GetTimeStamp(DateTime.Now) <= start)
            {
                server.Start();
            }
            server.Stop();
            /*using (var server = new BackgroundJobServer(options, storage))
            {
                Console.WriteLine("Hangfire Server started. Press any key to exit...");
                Console.ReadKey();
            }*/
        }

        public static int GetTimeStamp(DateTime date)
        {
           return (int) date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
