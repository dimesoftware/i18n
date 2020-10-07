namespace System.Globalization
{
    /// <summary>
    /// Extensions for the <see cref="DateTime"/> class
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts the instance into a local date time
        /// </summary>
        /// <param name="dateTime">The current instance of the date time</param>
        /// <param name="timeZone">The time zone to convert to</param>
        /// <returns>The same date time but expressed in local time</returns>
        public static DateTime ToLocal(this DateTime dateTime, string timeZone = null)
        {
            IDateTimeConverter converter = new LocalDateTimeConverter(timeZone);
            return converter.Convert(dateTime);
        }

        /// <summary>
        /// Converts the instance into a local date time
        /// </summary>
        /// <param name="dateTime">The current instance of the date time</param>
        /// <param name="timeZone">The time zone to convert to</param>
        /// <returns>The same date time but expressed in local time</returns>
        public static DateTime? ToLocal(this DateTime? dateTime, string timeZone = null)
        {
            if (dateTime == null)
                return null;

            IDateTimeConverter converter = new LocalDateTimeConverter(timeZone);
            return converter.Convert((DateTime)dateTime);
        }

        /// <summary>
        /// Converts the local date time into a UTC time
        /// </summary>
        /// <param name="dateTime">The current instance of the date time</param>
        /// <param name="timeZone">The time zone to convert from</param>
        /// <returns>The same date time but expressed in UTC time</returns>
        public static DateTime ToUtc(this DateTime dateTime, string timeZone = null)
        {
            IDateTimeConverter converter = new UtcDateTimeConverter(timeZone);
            return converter.Convert(dateTime);
        }

        /// <summary>
        /// Converts the local date time into a UTC time
        /// </summary>
        /// <param name="dateTime">The current instance of the date time</param>
        /// <param name="timeZone">The time zone to convert from</param>
        /// <returns>The same date time but expressed in UTC time</returns>
        public static DateTime? ToUtc(this DateTime? dateTime, string timeZone = null)
        {
            if (dateTime == null)
                return null;

            IDateTimeConverter converter = new UtcDateTimeConverter(timeZone);
            return converter.Convert((DateTime)dateTime);
        }
    }
}