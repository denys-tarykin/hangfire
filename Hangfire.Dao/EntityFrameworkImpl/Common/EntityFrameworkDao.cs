using System.Data.Entity;
using Hangfire.Common;
using Hangfire.Common.Exceptions;
using Hangfire.Dao.Common;

namespace Hangfire.Dao.EntityFrameworkImpl.Common
{
    public abstract class EntityFrameworkDao : IDao
    {
        [TransientDependency]
        protected virtual IDbSessionHolder GetSessionHolder() { return null; }


        protected DbContext GetDbContext()
        {
            object session = GetSessionHolder().GetSession(GetType());
            if (!(session is DbContext))
            {
                throw new DependencyResolutionException("DB_SESSION_SHOULD_BE_ANINSTANCE_OF_DBCONTEXT");
            }
            return session as DbContext;
        }
    }
}