using System.Web.Http;
using Hangfire.Common;
using Hangfire.Services.Api;
using Hangfire.Web.Api.DtoModel.Common;
using Microsoft.Practices.Unity;

namespace Hangfire.Web.Api.Controllers
{
    [RoutePrefix("auth")]
    public class AuthController : ApiController
    {
        [Dependency]
        public IDelayTaskService DelayTaskService { get; set; }
        [Dependency]
        public IFileSystemStorage FileSystemStorage { get; set; }

        [Route("get_status")]
        public DtoModelOutgoing GetStatus()
        {
            for (var i = 1; i < 7000; i++)
            {
                foreach (var picture in FileSystemStorage.GetAllFiles())
                {
                    DelayTaskService.ApplyWatermark(picture);
                }
            }
            
            
            return new DtoModelOutgoing((IPayload)null);
        }
    }
}
