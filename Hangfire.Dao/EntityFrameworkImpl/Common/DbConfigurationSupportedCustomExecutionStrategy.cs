using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Common.Logging;

namespace Hangfire.Dao.EntityFrameworkImpl.Common
{
    public class DbConfigurationSupportedCustomExecutionStrategy : DbConfiguration
    {
        public DbConfigurationSupportedCustomExecutionStrategy()
        {
            SetExecutionStrategy("System.Data.SqlClient", () =>  (IDbExecutionStrategy)new DefaultExecutionStrategy());
        }

        public static bool SuspendExecutionStrategy
        {
            get
            {
                return (bool?)CallContext.LogicalGetData("SuspendExecutionStrategy") ?? false;
            }
            set
            {
                CallContext.LogicalSetData("SuspendExecutionStrategy", value);
            }
        }
    }
}