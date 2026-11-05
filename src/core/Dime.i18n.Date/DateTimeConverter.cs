using NodaTime;
using NodaTime.TimeZones;

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
        /// <param name="timeZone">The time zone to use: any TZDB id (canonical, alias, UTC or Etc/*) or a Windows id</param>
        protected DateTimeConverter(string timeZone)
        {
            if (string.IsNullOrEmpty(timeZone))
                return;

            TimeZone = ResolveZoneId(timeZone) ?? throw new ArgumentException("Invalid time zone", nameof(timeZone));
        }

        protected bool UseCurrentCulture => string.IsNullOrEmpty(TimeZone);
        protected string TimeZone { get; }

        /// <summary>
        /// The TZDB id the conversions run on: the id itself when TZDB knows it, else the IANA zone a Windows id maps to.
        /// </summary>
        /// <param name="timeZone">The requested time zone</param>
        /// <returns>A TZDB zone id, or null when the id is neither</returns>
        private static string ResolveZoneId(string timeZone)
        {
            if (DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZone) != null)
                return timeZone;

            return TzdbDateTimeZoneSource.Default.WindowsMapping.PrimaryMapping.TryGetValue(timeZone, out string ianaZone) ? ianaZone : null;
        }
    }
}
