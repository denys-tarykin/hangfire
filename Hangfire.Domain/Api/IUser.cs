using Hangfire.Domain.Api.Common;

namespace Hangfire.Domain.Api
{
    public interface IUser : IDbDomain, IIdentifiable
    {
        string Username { get; set; }
        string Email { get; set; }
        string Password { get; set; }
    }
}