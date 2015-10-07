using System;

namespace Hangfire.Web.Api.DtoModel.Common
{
    public class DtoConverter
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        ///     Convert timestamp in milliseconds to DateTime object in UTC.
        /// </summary>
        /// <param name="valueInMs">The timestamp value in milliseconds</param>
        /// <returns>The DateTime object in UTC.</returns>
        public static DateTime ToDateTime(UInt64 valueInMs)
        {
            return Epoch.AddMilliseconds(valueInMs);
        }

        /// <summary>
        ///     Convert DateTime object object to timestamp in UTC.
        /// </summary>
        /// <param name="valueInUTC">The DateTime object in UTC.</param>
        /// <returns>The timestamp value in milliseconds</returns>
        public static UInt64? ToTimestampMs(DateTime? valueInUTC)
        {
            if (valueInUTC.HasValue)
                return (UInt64)(valueInUTC.Value - Epoch).TotalMilliseconds;

            return null;
        } 
    }
}