using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Http.Dependencies;
using Common.Logging;
using Hangfire.Common;
using Hangfire.Dao.EntityFrameworkImpl.Session;
using HangfireApplication.Web.Api;
using Microsoft.Practices.Unity;

namespace HangfireApplication.Web.Api
{
    public class UnityResolver : IDependencyResolver
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     Private reference to the unity container
        ///     to register instances or take other actions
        ///     on the container.
        /// </summary>
        private IUnityContainer _container;

        public UnityResolver(IUnityContainer container)
        {
            this._container = container;
        }

        void IDisposable.Dispose()
        {
            _container.Dispose();
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope()
        {
            var childContainer = _container.CreateChildContainer();
            HttpContext.Current.Items[WebApiConst.RequestUnityContainerAttributeName] = childContainer;
            return new UnityResolver(childContainer);
        }

    }
}