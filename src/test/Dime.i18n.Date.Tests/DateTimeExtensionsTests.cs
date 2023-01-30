using Xunit;

namespace System.Globalization.Tests
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void DateTimeExtensions_ToUtc_ShouldConvert()
        {
            DateTime dt = new(2020, 10, 7, 15, 0, 0);
            DateTime utcDt = dt.ToUtc("Europe/Brussels");
            Assert.True(utcDt == new DateTime(2020, 10, 7, 13, 0, 0));
        }

        [Fact]
        public void DateTimeExtensions_NullableDateTime_IsNotNull_ToUtc_ShouldConvert()
        {
            DateTime? dt = new DateTime(2020, 10, 7, 15, 0, 0);
            DateTime? utcDt = dt.ToUtc("Europe/Brussels");
            Assert.True(utcDt == new DateTime(2020, 10, 7, 13, 0, 0));
        }

        [Fact]
        public void DateTimeExtensions_NullableDateTime_IsNull_ToUtc_ShouldReturnNull()
        {
            DateTime? dt = null;
            DateTime? utcDt = dt.ToUtc("Europe/Brussels");
            Assert.True(utcDt == null);
        }

        [Fact]
        public void DateTimeExtensions_ToLocal_ShouldConvert()
        {
            DateTime dt = new(2020, 10, 7, 13, 0, 0);
            DateTime utcDt = dt.ToLocal("Europe/Brussels");
            Assert.True(utcDt == new DateTime(2020, 10, 7, 15, 0, 0));
        }

        [Fact]
        public void DateTimeExtensions_NullableDateTime_IsNotNull_ToLocal_ShouldConvert()
        {
            DateTime? dt = new DateTime(2020, 10, 7, 13, 0, 0);
            DateTime? utcDt = dt.ToLocal("Europe/Brussels");
            Assert.True(utcDt == new DateTime(2020, 10, 7, 15, 0, 0));
        }

        [Fact]
        public void DateTimeExtensions_NullableDateTime_IsNull_ToLocal_ShouldReturnNull()
        {
            DateTime? dt = null;
            DateTime? utcDt = dt.ToLocal("Europe/Brussels");
            Assert.True(utcDt == null);
        }
    }
}