using NodaTime.TimeZones;
using System.Linq;

namespace System.Globalization
{
    public abstract class DateTimeConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeConverter"/> class
        /// </summary>
        protected DateTimeConverter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeConverter"/> class
        /// </summary>
        /// <param name="timeZone">The time zone to use</param>
        protected DateTimeConverter(string timeZone)
        {
            if (string.IsNullOrEmpty(timeZone))
                return;

            TimeZone = TzdbDateTimeZoneSource.Default.ZoneLocations.Any(x => x.ZoneId == timeZone) ? timeZone : throw new ArgumentException("Invalid time zone", nameof(timeZone));
        }

        protected bool UseCurrentCulture => string.IsNullOrEmpty(TimeZone);
        protected string TimeZone { get; }
    }
}