using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Reflection;
using Common.Logging;
using Hangfire.Dao.EntityFrameworkImpl.Common;
using Hangfire.Domain.EntityFrameworkImpl;

namespace Hangfire.Dao.EntityFrameworkImpl.Session
{
    public class DbModelHolder
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private DbCompiledModel _compiledModel;
        private DbConnection _dbConnection;

        public string ConnectionString { get; set; }

        public DbConnection GetConnection()
        {
            if (_dbConnection == null)
            {
                _dbConnection = new SqlConnection(ConnectionString);
                _logger.Info("Created new DB connection");
            }
            return _dbConnection;
        }

        private void CreateModel()
        {
            var modelBuilder = new DbModelBuilder();
            modelBuilder.Entity<UserImpl>();
            _logger.Info("Constructed DB model");
            var model = modelBuilder.Build(GetConnection());
            _logger.Info("Compiled DB model");
            _compiledModel = model.Compile();
        }

        public DbCompiledModel GetCompiledDbModel()
        {
            if (_compiledModel == null)
            {
                DbConfiguration.SetConfiguration(new DbConfigurationSupportedCustomExecutionStrategy());
                CreateModel();
            }
            return _compiledModel;
        } 
    }
}