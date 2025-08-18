namespace BaseProject.Application.Common.Utilities
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Converts a DateTime to UTC.
        /// </summary>
        public static DateTime ToUtc(DateTime dateTime) => dateTime.ToUniversalTime();

        /// <summary>
        /// Formats a DateTime using the specified format string.
        /// </summary>
        public static string FormatDate(DateTime dateTime, string format = "yyyy-MM-dd")
            => dateTime.ToString(format);

        /// <summary>
        /// Returns the difference in days between two dates.
        /// </summary>
        public static int DaysBetween(DateTime start, DateTime end) => (end - start).Days;
    }
}
