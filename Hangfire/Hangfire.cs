using Hangfire;
using Hangfire.SqlServer;
using Owin;

namespace HangfireApplication.Web.Api
{
    public class Hangfire
    {
        public static void ConfigureHangfire(IAppBuilder app)
        {
            app.UseHangfire(config =>
            {
                config.UseSqlServerStorage("HangfireStorageDbContext");
                config.UseServer();
                config.UseDashboardPath("/dashboard");
                config.UseAuthorizationFilters(); //allow all users to access the dashboard
            });
        }
    }
}