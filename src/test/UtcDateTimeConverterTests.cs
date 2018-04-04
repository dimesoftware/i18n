using System;
using System.Globalization;
using System.Threading;
using Xunit;

namespace Dime.Utilities.Date.Tests
{
    public class UtcDateTimeConverterTests
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public UtcDateTimeConverterTests()
        {
        }

        #endregion Constructor

        [Fact]
        public void UtcDateTimeConverter_Constructor_Default()
        {
            UtcDateTimeConverter converter = new UtcDateTimeConverter();
        }

        [Fact]
        public void UtcDateTimeConverter_Constructor_TimezoneIsEmpty_Instantiates()
        {
            UtcDateTimeConverter converter = new UtcDateTimeConverter(string.Empty);
        }

        [Fact]
        public void UtcDateTimeConverter_Constructor_TimezoneIsNull_Instantiates()
        {
            UtcDateTimeConverter converter = new UtcDateTimeConverter(null);
        }

        [Fact]
        public void UtcDateTimeConverter_Constructor_TimezoneIsWrong_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new UtcDateTimeConverter("Europe/Zakkemakke"));
        }

        [Fact]
        public void UtcDateTimeConverter_ConvertToUtc_DateTime_UseCustomTimezone_Success()
        {
            const string inputValue = "2016-12-31 15:00";
            string outputValue = "";
            const string timeZone = "Europe/Brussels";

            UtcDateTimeConverter converter = new UtcDateTimeConverter(timeZone);

            string exactFormat = "yyyy-MM-dd HH:mm";
            if (DateTime.TryParseExact(inputValue, exactFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var output))
            {
                Console.WriteLine(output);
                DateTime convertedDate = converter.ConvertToUtc(DateTime.SpecifyKind(output, DateTimeKind.Utc));
                Console.WriteLine(convertedDate);
                outputValue = convertedDate.ToString(exactFormat);
                Console.WriteLine(outputValue);
            }

            Assert.True(outputValue == "2016-12-31 14:00");
        }

        [Fact]
        public void UtcDateTimeConverter_ConvertToUtc_DateTime_UseCurrentTimezone_Success()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-BE");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("nl-BE");

            const string inputValue = "2016-12-31 15:00";
            string outputValue = "";

            UtcDateTimeConverter converter = new UtcDateTimeConverter();
            string exactFormat = "yyyy-MM-dd HH:mm";
            if (DateTime.TryParseExact(inputValue, exactFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var output))
            {
                Console.WriteLine(output);
                DateTime convertedDate = converter.ConvertToUtc(DateTime.SpecifyKind(output, DateTimeKind.Utc));
                Console.WriteLine(convertedDate);
                outputValue = convertedDate.ToString(exactFormat);
                Console.WriteLine(outputValue);
            }

            Assert.True(outputValue == "2016-12-31 16:00");
        }

        [Fact]
        public void UtcDateTimeConverter_ConvertToUtc_Entity_Success()
        {
        }

        [Fact]
        public void UtcDateTimeConverter_ConvertToLocalTime_DateTime_UseCustomTimezone_Success()
        {
            const string inputValue = "2016-12-31 15:00";
            string outputValue = "";
            const string timeZone = "Europe/Brussels";

            UtcDateTimeConverter converter = new UtcDateTimeConverter(timeZone);

            string exactFormat = "yyyy-MM-dd HH:mm";
            if (DateTime.TryParseExact(inputValue, exactFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var output))
            {
                Console.WriteLine(output);
                DateTime convertedDate = converter.ConvertToLocalTime(DateTime.SpecifyKind(output, DateTimeKind.Utc));
                Console.WriteLine(convertedDate);
                outputValue = convertedDate.ToString(exactFormat);
                Console.WriteLine(outputValue);
            }

            Assert.True(outputValue == "2016-12-31 16:00");
        }

        [Fact]
        public void UtcDateTimeConverter_ConvertToLocalTime_DateTime_UseCurrentTimezone_Success()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-BE");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("nl-BE");

            const string inputValue = "2016-12-31 15:00";
            string outputValue = "";

            UtcDateTimeConverter converter = new UtcDateTimeConverter();
            string exactFormat = "yyyy-MM-dd HH:mm";
            if (DateTime.TryParseExact(inputValue, exactFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var output))
            {
                Console.WriteLine(output);
                DateTime convertedDate = converter.ConvertToLocalTime(DateTime.SpecifyKind(output, DateTimeKind.Utc));
                Console.WriteLine(convertedDate);
                outputValue = convertedDate.ToString(exactFormat);
                Console.WriteLine(outputValue);
            }

            Assert.True(outputValue == "2016-12-31 16:00");
        }

        [Fact]
        public void UtcDateTimeConverter_ConvertToLocalTime_Entity_Success()
        {
        }
    }
}