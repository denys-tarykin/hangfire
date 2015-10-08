using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Hangfire.Common
{
    public interface ITransactionalMethodHandler : ICallHandler
    {

    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class TransactionalAttribute : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return container.Resolve<ITransactionalMethodHandler>();
        }
    }
}