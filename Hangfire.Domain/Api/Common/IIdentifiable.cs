namespace Hangfire.Domain.Api.Common
{

    public enum EntityStatus
    {
        Active = 1,
        Deleted = 2
    }

    public interface IIdentifiable
    {
        long Id { get; set; }
        EntityStatus EntityStatus { get; set; }
    }
}