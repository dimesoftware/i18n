using NodaTime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Dime.Utilities
{
    /// <summary>
    /// Converts each <see cref="DateTime" /> or its nullable equivalent field to a UTC
    /// </summary>
    public class UtcDateTimeConverter
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UtcDateTimeConverter"/> class
        /// </summary>
        public UtcDateTimeConverter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UtcDateTimeConverter"/> class
        /// </summary>
        /// <param name="timeZone">The time zone to use</param>
        public UtcDateTimeConverter(string timeZone)
        {
            if (string.IsNullOrEmpty(timeZone))
                return;

            TimeZone = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default.ZoneLocations.Any(x => x.ZoneId == timeZone)
                ? timeZone
                : throw new ArgumentException("Invalid time zone", nameof(timeZone));
        }

        #endregion Constructor

        #region Properties

        private bool UseCurrentCulture
            => string.IsNullOrEmpty(TimeZone);

        private string TimeZone { get; }

        #endregion Properties

        #region Methods

        #region To Local Time

        /// <summary>
        /// Converts the date time to the local time
        /// </summary>
        /// <param name="dt">The date time to convert</param>
        /// <returns>The localized date time</returns>
        public DateTime ConvertToLocalTime(DateTime dt)
        {
            if (UseCurrentCulture)
            {
                CultureInfo currentCulture = CultureInfo.CurrentUICulture;
                RegionInfo regionInfo = new RegionInfo(currentCulture.Name);

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

        /// <summary>
        /// Converts the date time to the local time
        /// </summary>
        /// <param name="entity">The date time to convert</param>
        /// <returns>The localized date time</returns>
        public void ConvertToLocalTime<T>(T entity)
        {
            if (entity == null)
                return;

            Func<PropertyInfo, bool> dateProp = x =>
                (x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?))
                && x.GetCustomAttribute<DateTimeKindAttribute>() != null;

            IEnumerable<PropertyInfo> properties = entity.GetType().GetProperties().Where(dateProp);

            CultureInfo currentCulture = CultureInfo.CurrentUICulture;
            RegionInfo regionInfo = new RegionInfo(currentCulture.Name);

            IEnumerable<string> zoneIds = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default.ZoneLocations
                .Where(x => String.Compare(x.CountryCode, regionInfo.TwoLetterISORegionName, StringComparison.OrdinalIgnoreCase) == 0)
                .Select(x => x.ZoneId);

            foreach (PropertyInfo property in properties)
            {
                DateTimeKindAttribute attr = property.GetCustomAttribute<DateTimeKindAttribute>();

                DateTime? dt = property.PropertyType == typeof(DateTime?)
                    ? (DateTime?)property.GetValue(entity)
                    : (DateTime)property.GetValue(entity);

                if (dt == null)
                    continue;

                if (!zoneIds.Any())
                    continue;

                DateTime dateTime = DateTime.SpecifyKind(dt.Value, attr.Kind);
                Instant dateTimeInstant = Instant.FromDateTimeUtc(dateTime);

                DateTimeZone timeZone = DateTimeZoneProviders.Tzdb[zoneIds.FirstOrDefault()];
                ZonedDateTime zonedDateTime = dateTimeInstant.InZone(timeZone);

                DateTime localDateTime = zonedDateTime.ToDateTimeUnspecified();
                property.SetValue(entity, localDateTime);
            }
        }

        #endregion To Local Time

        #region To UTC Time

        /// <summary>
        /// Converts the date time to  UTC time
        /// </summary>
        /// <param name="dt">The date time to convert</param>
        /// <returns>The UTC date time</returns>
        public DateTime ConvertToUtc(DateTime dt)
        {
            // Fork this method into the use of the current culture to do the utc conversion
            if (UseCurrentCulture)
            {
                CultureInfo currentCulture = CultureInfo.CurrentUICulture;
                RegionInfo regionInfo = new RegionInfo(currentCulture.Name);

                IEnumerable<string> zoneIds = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default.ZoneLocations
                    .Where(x => String.Compare(x.CountryCode, regionInfo.TwoLetterISORegionName, StringComparison.OrdinalIgnoreCase) == 0)
                    .Select(x => x.ZoneId);

                if (zoneIds.Any())
                {
                    DateTime dateTime = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
                    Instant dateTimeInstant = Instant.FromDateTimeUtc(dateTime);

                    DateTimeZone timeZone = DateTimeZoneProviders.Tzdb[zoneIds.FirstOrDefault()];
                    ZonedDateTime zonedDateTime = dateTimeInstant.InZone(timeZone);

                    DateTime localDateTime = zonedDateTime.ToDateTimeUnspecified();
                    return localDateTime;
                }

                return dt;
            }
            else
            {
                // Get local DateTime instance into a LocalDateTime object
                LocalDateTime localDateTime = new LocalDateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute);

                // Get the users' time zone
                IDateTimeZoneProvider timeZoneProvider = DateTimeZoneProviders.Tzdb;
                DateTimeZone usersTimezone = timeZoneProvider[TimeZone];

                // Format the local DateTime instance with the time zones
                ZonedDateTime zonedDbDateTime = localDateTime.InZoneLeniently(usersTimezone);

                // At this point we have all information to convert to UTC: release the kraken!
                return zonedDbDateTime.ToDateTimeUtc();                
            }
        }

        #endregion To UTC Time

        #endregion Methods
    }
}