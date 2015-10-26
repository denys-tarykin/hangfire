using System.Linq;
using System.Web.Http;
using Hangfire.Common;
using Hangfire.Dao.EntityFrameworkImpl.Common;
using Hangfire.Dao.EntityFrameworkImpl.Session;
using Hangfire.Services;
using HangfireApplication.Web.Api;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace HangfireApplication.Web.Api
{
    public class UnityConfig
    {
        public static IUnityContainer Register(HttpConfiguration config)
        {
            IUnityContainer unityContainer = CreateAndConfigureContainer();
            config.DependencyResolver = new UnityResolver(unityContainer);
          
            // ----- OTHER COMPONENTS ------
            // Registering Transient Dependency resolver
            unityContainer.RegisterType<ITransientDependencyInjector, WebRequestDependencyInjector>(
                new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ITransactionalMethodHandler, TransactionalMethodHandler>(
               new ContainerControlledLifetimeManager(), UnityUtils.InterceptorSupport);
            unityContainer.RegisterType<DbModelHolder>(
                new ContainerControlledLifetimeManager(), UnityUtils.InterceptorSupport);
            // Registering Services
            UnityUtils.RegisterServices(unityContainer);
            UnityUtils.RegisterDao(unityContainer);

            RegisterTransientEntities(unityContainer);
            return unityContainer;
        }

        private static IUnityContainer CreateAndConfigureContainer()
        {
            IUnityContainer rootUnityContainer = new UnityContainer();
            rootUnityContainer.AddNewExtension<Interception>();
            return rootUnityContainer;
        }

        private static void RegisterServices(IUnityContainer unityContainer)
        {
            unityContainer.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(t => t.Namespace == "Hangfire.Services.Impl"),
                WithMappings.FromMatchingInterface, WithName.Default, WithLifetime.ContainerControlled,
                getInjectionMembers: t => new InjectionMember[]
                {                    
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<PolicyInjectionBehavior>(),
                });
        }
        
        private static void RegisterDao(IUnityContainer unityContainer)
        {
            unityContainer.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(t => t.Namespace == "Hangfire.Dao.EntityFrameworkImpl.Dao"),
                WithMappings.FromMatchingInterface, WithName.Default, WithLifetime.ContainerControlled,
                getInjectionMembers: t => new InjectionMember[]
                {                    
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<PolicyInjectionBehavior>(),
                });
        }
        private static void RegisterTransientEntities(IUnityContainer unityContainer)
        {
            // Register DB context entities            
            unityContainer.RegisterType<IDbSessionHolder, DbSessionHolder>(
                new HierarchicalLifetimeManager(), UnityUtils.InterceptorSupport);
        }
    }
}