using System;
using System.Collections.Generic;
using System.Linq;
using Hangfire.Dao.Common;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Hangfire.Services
{
    public class UnityUtils
    {
        public static readonly InjectionMember[] InterceptorSupport = new InjectionMember[]
        {
            new Interceptor<VirtualMethodInterceptor>(),
            new InterceptionBehavior<PolicyInjectionBehavior>(),
        };

        public static readonly IList<Type> ExcludedInterfaces = new List<Type>(new Type[] { typeof(IDao) });

        public static bool MatchingInterfaceFilter(Type interfaceType, Object implementationTypeObj)
        {
            var implementationType = implementationTypeObj as Type;
            if (ExcludedInterfaces.Contains(interfaceType)) return false;
            var interfaceName = interfaceType.Name.StartsWith("I")
                ? interfaceType.Name.Substring(1)
                : interfaceType.Name;
            return implementationType.Name.Contains(interfaceName);
        }

        /// <summary>
        /// Returns interface\interfaces which name without leading "I" is containing in implementation name.
        /// </summary>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        public static IEnumerable<Type> FromMatchingInterface(Type implementationType)
        {
            return implementationType.FindInterfaces(MatchingInterfaceFilter, implementationType);
        }

        public static void RegisterServices(IUnityContainer unityContainer)
        {
            unityContainer.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(t => t.Namespace.StartsWith("Hangfire.Services.Impl") && !t.IsAbstract),
                FromMatchingInterface, WithName.Default, WithLifetime.ContainerControlled,
                getInjectionMembers: t => InterceptorSupport);
        }

        public static void RegisterDao(IUnityContainer unityContainer)
        {
            unityContainer.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(t => t.Namespace.StartsWith("Hangfire.Dao.EntityFrameworkImpl.Dao") && !t.IsAbstract),
                FromMatchingInterface, WithName.Default, WithLifetime.ContainerControlled,
                getInjectionMembers: t => InterceptorSupport);
        }
    }
}