using NodaTime;
using System.Collections.Generic;
using System.Linq;

namespace System.Globalization
{
    /// <summary>
    /// Converts each <see cref="DateTime" /> or its nullable equivalent field to a UTC
    /// </summary>
    public class LocalDateTimeConverter : DateTimeConverter, IDateTimeConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDateTimeConverter"/> class
        /// </summary>
        public LocalDateTimeConverter() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDateTimeConverter"/> class
        /// </summary>
        /// <param name="timeZone">The time zone to use</param>
        public LocalDateTimeConverter(string timeZone) : base(timeZone)
        {
        }

        /// <summary>
        /// Converts the date time to the local time
        /// </summary>
        /// <param name="dt">The date time to convert</param>
        /// <returns>The localized date time</returns>
        public DateTime Convert(DateTime dt)
        {
            if (UseCurrentCulture)
            {
                CultureInfo currentCulture = CultureInfo.CurrentUICulture;
                RegionInfo regionInfo = new(currentCulture.Name);

                IEnumerable<string> zoneIds = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default.ZoneLocations
                    .Where(x => string.Compare(x.CountryCode, regionInfo.TwoLetterISORegionName, true) == 0)
                    .Select(x => x.ZoneId);

                if (!zoneIds.Any())
                    return dt;

                DateTime dateTime = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
                Instant dateTimeInstant = Instant.FromDateTimeUtc(dateTime);

                DateTimeZone timeZone = DateTimeZoneProviders.Tzdb[zoneIds.FirstOrDefault()];
                ZonedDateTime zonedDateTime = dateTimeInstant.InZone(timeZone);

                return zonedDateTime.ToDateTimeUnspecified();
            }
            else
            {
                DateTime dateTime = DateTime.SpecifyKind(dt, DateTimeKind.Utc);

                // Get instant from DateTime instance
                Instant instant = Instant.FromDateTimeUtc(dateTime);

                // Get user time zone
                IDateTimeZoneProvider timeZoneProvider = DateTimeZoneProviders.Tzdb;
                DateTimeZone usersTimezone = timeZoneProvider[TimeZone];

                // Set the instant to the local time zone
                ZonedDateTime usersZonedDateTime = instant.InZone(usersTimezone);
                DateTime localDateTime = usersZonedDateTime.ToDateTimeUnspecified();

                return localDateTime;
            }
        }
    }
}