using NodaTime;
using System.Collections.Generic;
using System.Linq;

namespace System.Globalization
{
    /// <summary>
    /// Converts each <see cref="DateTime" /> or its nullable equivalent field to a UTC
    /// </summary>
    public partial class UtcDateTimeConverter : DateTimeConverter, IDateTimeConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UtcDateTimeConverter"/> class
        /// </summary>
        public UtcDateTimeConverter() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UtcDateTimeConverter"/> class
        /// </summary>
        /// <param name="timeZone">The time zone to use</param>
        public UtcDateTimeConverter(string timeZone) : base(timeZone)
        {
        }

        /// <summary>
        /// Converts the date time to  UTC time
        /// </summary>
        /// <param name="dt">The date time to convert</param>
        /// <returns>The UTC date time</returns>
        public DateTime Convert(DateTime dt)
        {
            // Fork this method into the use of the current culture to do the utc conversion
            if (UseCurrentCulture)
            {
                CultureInfo currentCulture = CultureInfo.CurrentUICulture;
                RegionInfo regionInfo = new(currentCulture.Name);

                IEnumerable<string> zoneIds = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default.ZoneLocations
                    .Where(x => string.Compare(x.CountryCode, regionInfo.TwoLetterISORegionName, StringComparison.OrdinalIgnoreCase) == 0)
                    .Select(x => x.ZoneId);

                if (!zoneIds.Any())
                    return dt;

                DateTime dateTime = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
                Instant dateTimeInstant = Instant.FromDateTimeUtc(dateTime);

                DateTimeZone timeZone = DateTimeZoneProviders.Tzdb[zoneIds.FirstOrDefault()];
                ZonedDateTime zonedDateTime = dateTimeInstant.InZone(timeZone);

                DateTime localDateTime = zonedDateTime.ToDateTimeUnspecified();
                return localDateTime;
            }
            else
            {
                // Get local DateTime instance into a LocalDateTime object
                LocalDateTime localDateTime = new(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute);

                // Get the users' time zone
                IDateTimeZoneProvider timeZoneProvider = DateTimeZoneProviders.Tzdb;
                DateTimeZone usersTimezone = timeZoneProvider[TimeZone];

                // Format the local DateTime instance with the time zones
                ZonedDateTime zonedDbDateTime = localDateTime.InZoneLeniently(usersTimezone);

                // At this point we have all information to convert to UTC: release the kraken!
                return zonedDbDateTime.ToDateTimeUtc();
            }
        }
    }
}