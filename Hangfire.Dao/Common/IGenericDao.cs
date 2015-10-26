using Hangfire.Domain.Api.Common;

namespace Hangfire.Dao.Common
{
    public interface IGenericDao<T> : IDao where T : IDbDomain
    {
        T Add(T entity, bool flush = false);
        void Delete(T model, bool flush = false);
        T Update(T entity, bool flush = false);
    }
}