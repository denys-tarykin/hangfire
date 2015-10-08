using System;
using System.Reflection;
using System.Threading;
using Common.Logging;
using Hangfire.Common.Exceptions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Hangfire.Common
{
    public class ThreadLocalDependencyInjector : ITransientDependencyInjector
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly string SlotName = "_unityContainer";
        private LocalDataStoreSlot _slot = Thread.GetNamedDataSlot(SlotName);
        private IUnityContainer UnityContainer
        {
            get
            {
                return (IUnityContainer)Thread.GetData(_slot);
            }
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IUnityContainer container = UnityContainer;
            Type targetType = null;
            object result = null;
            var logData =
                String.Format("ThreadLocalDependencyInjector input: {0}, input.MethodBase: {1} ", input, input.MethodBase);

            if (input.MethodBase is MethodInfo)
            {
                targetType = ((MethodInfo)input.MethodBase).ReturnType;
            }
            else
            {
                throw new DependencyResolutionException(String.Format("Can't apply TransientDependency injector to {0}. It could be applied only to methods.", input.MethodBase.Name));
            }
            try
            {
                result = container.Resolve(targetType);
            }
            catch (Exception e)
            {
                _logger.Info(String.Format("ThreadLocalDependencyInjector input data: {0}", logData));
                throw new DependencyResolutionException(String.Format("Unable to resolve transient dependency of type {0}. See inner exception for details", targetType.FullName), e);
            }
            return input.CreateMethodReturn(result);
        }

        public int Order { get; set; }
    }
}