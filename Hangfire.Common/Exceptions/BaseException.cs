using System;

namespace Hangfire.Common.Exceptions
{
    public class BaseException : Exception
    {
        public BaseException(String exceptionMessage)
            : base(exceptionMessage)
        {
        }

        public BaseException(String exceptionMessage, Exception exception)
            : base(exceptionMessage, exception)
        {
        }
    }
}