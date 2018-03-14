using NodaTime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Dime.Utilities
{
    /// <summary>
    /// Converts each DateTime or DateTime? field to a UTC
    /// </summary>
    public class UtcDateTimeConverter
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public UtcDateTimeConverter()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="timeZone"></param>
        public UtcDateTimeConverter(string timeZone)
        {
            if (string.IsNullOrEmpty(timeZone))
                return;
            else if (NodaTime.TimeZones.TzdbDateTimeZoneSource.Default.ZoneLocations.Any(x => x.ZoneId == timeZone))
                this.TimeZone = timeZone;
            else
                throw new ArgumentException("Invalid time zone", "timeZone");
        }

        #endregion Constructor

        #region Properties

        private bool UseCurrentCulture { get { return string.IsNullOrEmpty(this.TimeZone); } }
        private string TimeZone { get; }

        #endregion Properties

        #region Methods

        #region To Local Time

        /// <summary>
        ///
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DateTime ConvertToLocalTime(DateTime dt)
        {
            if (this.UseCurrentCulture)
            {
                CultureInfo currentCulture = CultureInfo.CurrentUICulture;
                RegionInfo regionInfo = new RegionInfo(currentCulture.Name);

                IEnumerable<string> zoneIds = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default.ZoneLocations
                    .Where(x => string.Compare(x.CountryCode, regionInfo.TwoLetterISORegionName, true) == 0)
                    .Select(x => x.ZoneId);

                if (zoneIds != null && zoneIds.Any())
                {
                    DateTime dateTime = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
                    Instant dateTimeInstant = Instant.FromDateTimeUtc(dateTime);

                    DateTimeZone timeZone = DateTimeZoneProviders.Tzdb[zoneIds.FirstOrDefault()];
                    ZonedDateTime zonedDateTime = dateTimeInstant.InZone(timeZone);

                    DateTime localDateTime = zonedDateTime.ToDateTimeUnspecified();
                    return localDateTime;
                }
                else
                {
                    return dt;
                }
            }
            else
            {
                DateTime dateTime = DateTime.SpecifyKind(dt, DateTimeKind.Utc);

                // Get instant from DateTime instance
                Instant instant = Instant.FromDateTimeUtc(dateTime);

                // Get user time zone
                IDateTimeZoneProvider timeZoneProvider = DateTimeZoneProviders.Tzdb;
                DateTimeZone usersTimezone = timeZoneProvider[this.TimeZone];

                // Set the instant to the local time zone
                ZonedDateTime usersZonedDateTime = instant.InZone(usersTimezone);
                DateTime localDateTime = usersZonedDateTime.ToDateTimeUnspecified();

                return localDateTime;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void ConvertToLocalTime<T>(T entity)
        {
            if (entity == null)
                return;

            Func<PropertyInfo, bool> dateProp = x => (x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?)) && x.GetCustomAttribute<DateTimeKindAttribute>() != null;
            IEnumerable<PropertyInfo> properties = entity.GetType().GetProperties().Where(dateProp);

            CultureInfo currentCulture = CultureInfo.CurrentUICulture;
            RegionInfo regionInfo = new RegionInfo(currentCulture.Name);

            IEnumerable<string> zoneIds = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default.ZoneLocations
                .Where(x => string.Compare(x.CountryCode, regionInfo.TwoLetterISORegionName, true) == 0)
                .Select(x => x.ZoneId);

            foreach (PropertyInfo property in properties)
            {
                DateTimeKindAttribute attr = property.GetCustomAttribute<DateTimeKindAttribute>();

                var dt = property.PropertyType == typeof(DateTime?)
                    ? (DateTime?)property.GetValue(entity)
                    : (DateTime)property.GetValue(entity);

                if (dt == null)
                    continue;

                if (zoneIds == null || !zoneIds.Any())
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
        ///
        /// </summary>
        /// <param name="dt"></param>
        public DateTime ConvertToUtc(DateTime dt)
        {
            // Fork this method into the use of the current culture to do the utc conversion
            if (this.UseCurrentCulture)
            {
                CultureInfo currentCulture = CultureInfo.CurrentUICulture;
                RegionInfo regionInfo = new RegionInfo(currentCulture.Name);

                IEnumerable<string> zoneIds = NodaTime.TimeZones.TzdbDateTimeZoneSource.Default.ZoneLocations
                    .Where(x => string.Compare(x.CountryCode, regionInfo.TwoLetterISORegionName, true) == 0)
                    .Select(x => x.ZoneId);

                if (zoneIds != null && zoneIds.Count() > 0)
                {
                    DateTime dateTime = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
                    Instant dateTimeInstant = Instant.FromDateTimeUtc(dateTime);

                    DateTimeZone timeZone = DateTimeZoneProviders.Tzdb[zoneIds.FirstOrDefault()];
                    ZonedDateTime zonedDateTime = dateTimeInstant.InZone(timeZone);

                    DateTime localDateTime = zonedDateTime.ToDateTimeUnspecified();
                    return localDateTime;
                }
                else
                    return dt;
            }
            else
            {
                // Get local DateTime instance into a LocalDateTime object
                LocalDateTime localDateTime = new LocalDateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute);

                // Get the users' time zone
                IDateTimeZoneProvider timeZoneProvider = DateTimeZoneProviders.Tzdb;
                DateTimeZone usersTimezone = timeZoneProvider[this.TimeZone];

                // Format the local DateTime instance with the time zones
                ZonedDateTime zonedDbDateTime = localDateTime.InZoneLeniently(usersTimezone);

                // At this point we have all information to convert to UTC: release the kraken!
                DateTime utcDateTime = zonedDbDateTime.ToDateTimeUtc();

                return utcDateTime;
            }
        }

        #endregion To UTC Time

        #endregion Methods
    }
}