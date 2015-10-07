namespace Hangfire.Domain.Api.Common
{
    public interface IDbDomain
    {
        byte[] RowVersion { get; set; }  
    }
}