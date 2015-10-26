using System.Web.Http;
using Hangfire.Common;
using Hangfire.Services.Api;
using HangfireApplication.Web.Api.DtoModel.Common;
using Microsoft.Practices.Unity;

namespace HangfireApplication.Web.Api.Controllers
{
    [RoutePrefix("demo")]
    public class DemoController : ApiController
    {
        [Dependency]
        public IDelayTaskService DelayTaskService { get; set; }
        [Dependency]
        public IFileSystemStorage FileSystemStorage { get; set; }

        [Route("create_jobs")]
        [HttpGet]
        public DtoModelOutgoing GetStatus()
        {
            for (var i = 1; i < 80; i++)
            {
                foreach (var picture in FileSystemStorage.GetAllFiles())
                {
                    //FileSystemStorage.ApplyWatermark(picture);
                    DelayTaskService.ApplyWatermark(picture);
                }
            }
            
            
            return new DtoModelOutgoing((IPayload)null);
        }

        [Route("start_process")]
        [HttpGet]
        public DtoModelOutgoing Start()
        {
            DelayTaskService.StartServer();
            return new DtoModelOutgoing((IPayload)null);
        }
    }
}
