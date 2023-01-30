using Xunit;

namespace System.Globalization.Tests
{
    public class DateTimeKindAttributeTests
    {
        [Fact]
        public void DateTimeKindAttribute_Constructor_Default()
        {
            DateTimeKindAttribute converter = new(DateTimeKind.Utc);
            Assert.True(converter.Kind == DateTimeKind.Utc);
        }

        [Fact]
        public void DateTimeKindAttribute_Apply_ParameterIsNull_DoesNothing()
        {
            DateTimeKindAttribute converter = new(DateTimeKind.Utc);
            Assert.True(converter.Kind == DateTimeKind.Utc);

            DateTime dt = new(2018, 1, 1, 12, 30, 00, DateTimeKind.Local);
            DateTimeTestClass dateTimeTestClass = null;
            DateTimeKindAttribute.Apply(dateTimeTestClass);
        }

        [Fact]
        public void DateTimeKindAttribute_Apply_ParameterIsNotNull_DateTimeIsConverted()
        {
            DateTimeKindAttribute converter = new(DateTimeKind.Utc);
            Assert.True(converter.Kind == DateTimeKind.Utc);

            DateTime dt = new(2018, 1, 1, 12, 30, 00, DateTimeKind.Local);
            DateTimeTestClass dateTimeTestClass = new(dt);

            DateTimeKindAttribute.Apply(dateTimeTestClass);
            Assert.True(dateTimeTestClass.MyDateTime.Kind == DateTimeKind.Utc);
        }

        [Fact]
        public void DateTimeKindAttribute_Apply_ParameterIsNotNull_DateTimeIsNull_DoesNothing()
        {
            DateTimeKindAttribute converter = new(DateTimeKind.Utc);
            Assert.True(converter.Kind == DateTimeKind.Utc);

            DateTime dt = new(2018, 1, 1, 12, 30, 00, DateTimeKind.Local);
            DateTimeTestClass dateTimeTestClass = new(dt);

            DateTimeKindAttribute.Apply(dateTimeTestClass);
        }

        private class DateTimeTestClass
        {
            [DateTimeKindAttribute(DateTimeKind.Utc)]
            // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
            // ReSharper disable once MemberCanBePrivate.Local
            public DateTime MyDateTime { get; set; }

            [DateTimeKindAttribute(DateTimeKind.Utc)]
            public DateTime? MyNullableDateTime { get; private set; }

            public DateTimeTestClass(DateTime dt)
            {
                MyDateTime = dt;
            }
        }
    }
}