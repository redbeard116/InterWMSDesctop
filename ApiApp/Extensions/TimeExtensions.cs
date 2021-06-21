using System;

namespace ApiApp.Extensions
{
    public static class TimeExtensions
    {
        public static long GetUnixTime(this DateTime winTime)
        {
            return new DateTimeOffset(winTime).ToUnixTimeSeconds();
        }

        public static DateTime GetNormalTime(this long unixDate)
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date = start.AddSeconds(unixDate).ToLocalTime();
            return date;
        }
    }
}
