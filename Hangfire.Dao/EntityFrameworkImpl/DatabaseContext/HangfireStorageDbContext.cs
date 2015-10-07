using System.Data.Entity;

namespace Hangfire.Dao.EntityFrameworkImpl.DatabaseContext
{
    public class HangfireStorageDbContext : DbContext
    {
        public HangfireStorageDbContext()
            : base("name=HangfireStorageDbContext")
        {

        }
    }
}