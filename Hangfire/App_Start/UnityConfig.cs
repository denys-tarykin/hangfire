using System.Linq;
using System.Web.Http;
using Hangfire.Common;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Hangfire.Web.Api
{
    public class UnityConfig
    {
        public static void Register(HttpConfiguration config)
        {
            IUnityContainer unityContainer = CreateAndConfigureContainer();
            config.DependencyResolver = new UnityResolver(unityContainer);
            // Registering Services
            RegisterServices(unityContainer);

            // ----- OTHER COMPONENTS ------
            // Registering Transient Dependency resolver
            unityContainer.RegisterType<ITransientDependencyInjector, WebRequestDependencyInjector>(
                new ContainerControlledLifetimeManager());
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
    }
}