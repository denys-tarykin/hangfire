using System;
using Hangfire.Services.Api;

namespace Hangfire.Services.Impl
{
    public class CalcService:ICalcService
    {
        public int Plus(int a, int b)
        {
            return a + b;
        }
    }
}