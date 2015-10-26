
using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using Hangfire.Common;
using Hangfire.Dao.EntityFrameworkImpl.Common;
using Hangfire.Dao.EntityFrameworkImpl.Session;
using Hangfire.Services;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Hangfire.Server
{
    public class UnityConfig
    {
        //private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public static IUnityContainer Register()
        {
            IUnityContainer unityContainer = CreateAndConfigureContainer();
            // Registering DAOs
            //UnityUtils.RegisterDao(unityContainer);
            RegisterDao(unityContainer);
            // Registering Services
            UnityUtils.RegisterServices(unityContainer);
            // Registering Controllers
            RegisterControllers(unityContainer);
            RegisterTransientEntities(unityContainer);
            Thread.AllocateNamedDataSlot(ThreadLocalDependencyInjector.SlotName);

            unityContainer.RegisterType<ITransientDependencyInjector, ThreadLocalDependencyInjector>(
                new ContainerControlledLifetimeManager());

            unityContainer.RegisterType<ITransactionalMethodHandler, TransactionalMethodHandler>(
                new ContainerControlledLifetimeManager(), UnityUtils.InterceptorSupport);
            // DB Model Holder
            unityContainer.RegisterType<DbModelHolder>(
                new ContainerControlledLifetimeManager(), UnityUtils.InterceptorSupport);
            return unityContainer;
        }

        private static void RegisterDao(IUnityContainer unityContainer)
        {
            unityContainer.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(t => t.Namespace.StartsWith("Hangfire.Dao.EntityFrameworkImpl.Dao") && !t.IsAbstract),
                UnityUtils.FromMatchingInterface, WithName.Default, WithLifetime.PerThread,
                getInjectionMembers: t => UnityUtils.InterceptorSupport); 
        }

        private static IUnityContainer CreateAndConfigureContainer()
        {
            IUnityContainer rootUnityContainer = new UnityContainer();
            rootUnityContainer.AddNewExtension<Interception>();
            return rootUnityContainer;
        }

        private static void RegisterControllers(IUnityContainer unityContainer)
        {
            unityContainer.RegisterTypes(
                AllClasses.FromLoadedAssemblies()
                    .Where(t => t.Namespace.StartsWith("Hangfire.Web.Api.Controllers")),
                WithMappings.None, WithName.Default, WithLifetime.Hierarchical, getInjectionMembers: t => UnityUtils.InterceptorSupport);
        }

        private static void RegisterTransientEntities(IUnityContainer unityContainer)
        {
            // Register DB context entities            
            unityContainer.RegisterType<IDbSessionHolder, DbSessionHolder>(
                new HierarchicalLifetimeManager(), UnityUtils.InterceptorSupport);
        }
        public static void ConfigureDefaults(IUnityContainer unityContainer)
        {      
            try
            {
                var dbModelHolder = unityContainer.Resolve<DbModelHolder>();
                dbModelHolder.ConnectionString = ConfigurationManager.ConnectionStrings["CommonDbContext"].ConnectionString;
            }
            catch (Exception e)
            {
               // _logger.Error("Can not create CommonDbContext container", e);
            }
        }
    }
}