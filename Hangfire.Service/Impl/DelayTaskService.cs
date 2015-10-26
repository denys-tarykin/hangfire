
using System;
using System.Threading;
using Hangfire.Services.Api;
using Hangfire.SqlServer;
using Microsoft.Practices.Unity;

namespace Hangfire.Services.Impl
{
    public class DelayTaskService : IDelayTaskService
    {
        [Dependency]
        public ICalcService CalcService { get; set; }
        
        [Dependency]
        public IFileSystemStorage FileSystemStorage { get; set; }
        
        public void Test(int a, int b)
        {
            var d = CalcService.Plus(a, b);
            BackgroundJob.Enqueue(() => Console.WriteLine(d));
        }

        public void ApplyWatermark(string picture)
        {
          //IFileSystemStorage storage = new FileSystemStorage();
            var client = new BackgroundJobClient();
           // client.Create()
            client.Enqueue<IFileSystemStorage>(file => file.ApplyWatermark(picture));
        }

        public void StartServer()
        {
            
            //Thread.Sleep(300000);
            //server.Stop();
        }
    }
}