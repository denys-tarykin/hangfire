using System;
using System.Reflection;
using System.Web;
using Hangfire.Common;
using Hangfire.Common.Exceptions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Hangfire.Web.Api
{
    public class WebRequestDependencyInjector : ITransientDependencyInjector
    {
        private IUnityContainer UnityContainer
        {
            get { return (IUnityContainer)HttpContext.Current.Items[WebApiConst.RequestUnityContainerAttributeName]; }
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IUnityContainer container = UnityContainer;
            Type targetType = null;
            object result = null;
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
                throw new DependencyResolutionException(String.Format("Unable to resolve transient dependency of type {0}. See inner exception for details", targetType.FullName), e);
            }
            return input.CreateMethodReturn(result);
        }

        public int Order { get; set; }
    }
}