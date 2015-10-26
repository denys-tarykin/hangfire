using Hangfire.Domain.Api.Common;

namespace Hangfire.Domain.Api
{
    public interface IAsset: IDbDomain, IIdentifiable
    {
        string Name { get; set; }
        string Path { get; set; }
    }
}