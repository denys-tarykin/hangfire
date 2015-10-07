using System;

namespace Hangfire.Common.Exceptions
{
    public class DependencyResolutionException : BaseException
    {
        public DependencyResolutionException(string exceptionMessage)
            : base(exceptionMessage)
        {
        }

        public DependencyResolutionException(string exceptionMessage, Exception exception)
            : base(exceptionMessage, exception)
        {
        }
    }
}