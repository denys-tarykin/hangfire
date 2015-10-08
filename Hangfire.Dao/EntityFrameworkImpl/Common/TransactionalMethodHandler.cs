using System;
using System.Data;
using System.Data.Entity;
using System.Reflection;
using Common.Logging;
using Hangfire.Common;
using Hangfire.Common.Exceptions;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Hangfire.Dao.EntityFrameworkImpl.Common
{
    public class TransactionalMethodHandler : ITransactionalMethodHandler
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [TransientDependency]
        public virtual IDbSessionHolder SessionHolder { get; set; }

        protected DbContext GetDbContext(Type wrappingClass)
        {
            object session = SessionHolder.GetSession(wrappingClass);
            if (!(session is DbContext))
            {
                throw new DependencyResolutionException("DB_SESSION_SHOULD_BE_ANINSTANCE_OF_DBCONTEXT");
            }
            return session as DbContext;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            DbContext dbContext = GetDbContext(input.Target.GetType());
            DbConfigurationSupportedCustomExecutionStrategy.SuspendExecutionStrategy = true;
            using (var transaction = dbContext.Database.BeginTransaction(IsolationLevel.Snapshot))
            //using (var transaction = dbContext.Database.BeginTransaction())
            {
                IMethodReturn result = getNext()(input, getNext);
                if (result.Exception == null)
                {
                    dbContext.SaveChanges();
                    transaction.Commit();
                }
                else
                {
                    _logger.Info(String.Format("Detected exception in transaction. Perform rollback. Exception: {0}",
                        result.Exception.Message),
                        result.Exception);

                    transaction.Rollback();
                }
                DbConfigurationSupportedCustomExecutionStrategy.SuspendExecutionStrategy = false;
                return result;
            }
        }

        public int Order { get; set; }
    }
}