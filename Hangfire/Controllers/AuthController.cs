using System.Web.Http;
using Hangfire.Common;
using Hangfire.Web.Api.DtoModel.Common;

namespace Hangfire.Web.Api.Controllers
{
    [RoutePrefix("auth")]
    public class AuthController : ApiController
    {
        [Route("get_status")]
        public DtoModelOutgoing GetStatus()
        {
            return new DtoModelOutgoing((IPayload)null);
        }
    }
}
