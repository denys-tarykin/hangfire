using Hangfire.Domain.Api.Common;

namespace Hangfire.Dao.Common
{
    public interface IGenericIdentifiableDao<T> : IGenericDao<T> where T : IIdentifiable, IDbDomain
    {
        T GetById(long Id);
        T AddOrUpdate(T entity, bool flush = false);
    }
}