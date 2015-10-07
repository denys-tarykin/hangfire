using System;

namespace Hangfire.Common
{
    public interface IDbSessionHolder
    {
        object GetSession(Type type);
    }
}