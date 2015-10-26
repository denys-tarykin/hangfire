using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using Hangfire.Common;
using Hangfire.Dao.EntityFrameworkImpl.Session;
using Hangfire.Services;
using Hangfire.Services.Api;
using Hangfire.Services.Impl;
using Microsoft.Practices.Unity;

namespace Hangfire.Server
{
    public class UnityJobActivator : JobActivator
    {
        //[ThreadStatic]
        private static IUnityContainer _container;

        public UnityJobActivator(IUnityContainer container)
        {
            _container = container.CreateChildContainer();
            //_container.RegisterType<DbModelHolder>(new PerResolveLifetimeManager());

            // _container.RegisterType<IFileSystemStorage, FileSystemStorage>(new PerResolveLifetimeManager());
            // _container.RegisterType<ITransientDependencyInjector, ThreadLocalDependencyInjector>(new PerResolveLifetimeManager());
             _container.RegisterType<IDbSessionHolder, DbSessionHolder>(new PerThreadLifetimeManager());
            try
            {
                var dbModelHolder = _container.Resolve<DbModelHolder>();
                dbModelHolder.ConnectionString = ConfigurationManager.ConnectionStrings["CommonDbContext"].ConnectionString;
            }
            catch (Exception e)
            {
                // _logger.Error("Can not create CommonDbContext container", e);
            }
        }

        public override object ActivateJob(Type type)
        {
            return _container.Resolve(type);
        }
    }

    public class ChildContainerPerJobFilterAttribute : JobFilterAttribute, IServerFilter
    {
        public ChildContainerPerJobFilterAttribute(UnityJobActivator unityJobActivator)
        {
            UnityJobActivator = unityJobActivator;
        }

        public UnityJobActivator UnityJobActivator { get; set; }

        public void OnPerformed(PerformedContext filterContext)
        {
            //UnityJobActivator.DisposeChildContainer();
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            //UnityJobActivator.CreateChildContainer();
        }
    }
}