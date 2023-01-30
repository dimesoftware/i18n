using System.Threading;
using Xunit;

namespace System.Globalization.Tests
{
    public class LocalDateTimeConverterTests
    {
        [Fact]
        public void LocalDateTimeConverter_Constructor_Default_ShouldNotThrowException()
            => Assert.True(new LocalDateTimeConverter() != null);

        [Fact]
        public void LocalDateTimeConverter_Constructor_TimezoneIsEmpty_ShouldNotThrowException()
            => Assert.True(new LocalDateTimeConverter(string.Empty) != null);

        [Fact]
        public void LocalDateTimeConverter_Constructor_TimezoneIsNull_ShouldNotThrowExceptions()
            => Assert.True(new LocalDateTimeConverter(null) != null);

        [Fact]
        public void LocalDateTimeConverter_Constructor_TimezoneIsWrong_ShouldThrowArgumentException()
            => Assert.Throws<ArgumentException>(() => new LocalDateTimeConverter("Europe/Atlantis"));

        [Fact]
        public void LocalDateTimeConverter_ConvertToLocalTime_DateTime_UseCustomTimezone_Success()
        {
            const string inputValue = "2016-12-31 15:00";
            string outputValue = "";
            const string timeZone = "Europe/Brussels";

            IDateTimeConverter converter = new LocalDateTimeConverter(timeZone);
            
            const string exactFormat = "yyyy-MM-dd HH:mm";
            if (DateTime.TryParseExact(inputValue, exactFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out DateTime output))
            {
                DateTime convertedDate = converter.Convert(DateTime.SpecifyKind(output, DateTimeKind.Utc));
                outputValue = convertedDate.ToString(exactFormat);
            }

            Assert.True(outputValue == "2016-12-31 16:00");
        }

        [Fact]
        public void LocalDateTimeConverter_ConvertToLocalTime_DateTime_UseCurrentTimezone_Success()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-BE");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("nl-BE");

            const string inputValue = "2016-12-31 15:00";
            string outputValue = "";

            IDateTimeConverter converter = new LocalDateTimeConverter();
            const string exactFormat = "yyyy-MM-dd HH:mm";
            if (DateTime.TryParseExact(inputValue, exactFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out DateTime output))
            {
                DateTime convertedDate = converter.Convert(DateTime.SpecifyKind(output, DateTimeKind.Utc));
                outputValue = convertedDate.ToString(exactFormat);
            }

            Assert.True(outputValue == "2016-12-31 16:00");
        }
    }
}