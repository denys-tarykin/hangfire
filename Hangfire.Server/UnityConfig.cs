using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using Hangfire.Common;
using Hangfire.Dao.EntityFrameworkImpl.Common;
using Hangfire.Dao.EntityFrameworkImpl.Session;
using Hangfire.Logging;
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
            UnityUtils.RegisterDao(unityContainer);
            // Registering Services
            UnityUtils.RegisterServices(unityContainer);
            // Registering Controllers
            RegisterControllers(unityContainer);

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

    }
}