using System;
using System.ComponentModel;
using Hangfire.Common;
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
            _container = container;
            _container.RegisterType<IFileSystemStorage, FileSystemStorage>(new PerResolveLifetimeManager());
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