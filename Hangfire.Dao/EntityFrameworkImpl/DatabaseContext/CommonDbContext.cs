using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using Common.Logging;
using Hangfire.Domain.EntityFrameworkImpl;

namespace Hangfire.Dao.EntityFrameworkImpl.DatabaseContext
{
    public class CommonDbContext : DbContext, IDisposable
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // DEPRECATED CONSTRUCTOR!
        public CommonDbContext()
            : base("name=CommonDbContext")
        {
            Init();
        }

        public CommonDbContext(DbCompiledModel model)
            : base(model)
        {
            Init();
        }

        private void Init()
        {
            Configuration.LazyLoadingEnabled = false;
            //Database.Log = s => _logger.Info(s);
            Database.SetInitializer<CommonDbContext>(null);
        }

        protected override void Dispose(bool disposing)
        {
            /*if (Database.Connection.State != ConnectionState.Closed)
            {
                Database.Connection.Close();
            }*/
            base.Dispose(disposing);
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserImpl>();
        }
    }
}