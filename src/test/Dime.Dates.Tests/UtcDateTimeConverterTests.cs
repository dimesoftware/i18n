using System;
using System.Globalization;
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
        public void UtcDateTimeConverter_StringToUtc_Success()
        {
            const string inputValue = "2016-12-31 15:00";
            string outputValue = "";
            const string timeZone = "Europe/Brussels";

            UtcDateTimeConverter converter = new UtcDateTimeConverter(timeZone);

            string exactFormat = "yyyy-MM-dd HH:mm";
            if (DateTime.TryParseExact(inputValue, exactFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var output))
            {
                Console.WriteLine(output);
                DateTime convertedDate = converter.ConvertToUtc(DateTime.SpecifyKind(output, DateTimeKind.Utc));
                Console.WriteLine(convertedDate);
                outputValue = convertedDate.ToString(exactFormat);
                Console.WriteLine(outputValue);
            }

            Assert.True(outputValue == "2016-12-31 14:00");
        }
    }
}