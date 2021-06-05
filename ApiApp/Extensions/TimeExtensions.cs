using System;

namespace ApiApp.Extensions
{
    public static class TimeExtensions
    {
        public static long GetUnixTime(this DateTime winTime)
        {
            return new DateTimeOffset(winTime).ToUnixTimeMilliseconds();
        }
        public static long? GetUnixTime(this DateTime? winTime)
        {
            return winTime.HasValue ? new DateTimeOffset(winTime.Value).ToUnixTimeMilliseconds() : (long?)null;
        }
        public static DateTime StartOfWeek(this DateTime date, DayOfWeek startOfWeek)
        {
            int diff = (7 + (date.DayOfWeek - startOfWeek)) % 7;
            return date.AddDays(-1 * diff).Date;
        }
        public static DateTime StartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
        public static DateTime StartOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 1, 1);
        }

        public static DateTime CurrentValueTime(string value)
        {
            var time = GetTime(value.Substring(0, 1));
            var opr = value.Substring(1, 1);
            var second = value.Substring(2);
            switch (opr)
            {
                case "+":
                    return time.AddSeconds(int.Parse(second));
                case "-":
                    return time.AddSeconds(-int.Parse(second));
            }
            return time;
        }

        private static DateTime GetTime(string value)
        {
            switch (value)
            {
                default:
                case "n":
                    return DateTime.Now;
                case "d":
                    return DateTime.Today;
                case "w":
                    return DateTime.Now.StartOfWeek(DayOfWeek.Monday);
                case "m":
                    return DateTime.Now.StartOfMonth();
                case "y":
                    return DateTime.Now.StartOfYear();
            }
        }
        public static DateTime GetNormalTime(this long unixDate)
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
            return date;
        }
    }
}
