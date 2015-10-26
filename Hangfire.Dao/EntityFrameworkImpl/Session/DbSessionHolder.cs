using System;
using Hangfire.Common;
using Hangfire.Dao.EntityFrameworkImpl.DatabaseContext;
using Microsoft.Practices.Unity;

namespace Hangfire.Dao.EntityFrameworkImpl.Session
{
    public class DbSessionHolder : IDbSessionHolder, IDisposable
    {
        [Dependency]
        public virtual DbModelHolder ModelHolder { get; set; }

        public CommonDbContext CommonDbContext { get; set; }

        public object GetSession(Type type)
        {
            if (CommonDbContext == null)
            {
                CommonDbContext = new CommonDbContext(ModelHolder.GetCompiledDbModel());
            }
            return CommonDbContext;
        }

        public void Dispose()
        {
            if (CommonDbContext != null)
            {
                CommonDbContext.Dispose();
            }
        }

    }
}