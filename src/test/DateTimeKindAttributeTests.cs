using System;
using Xunit;

namespace Dime.Utilities.Date.Tests
{
    public class DateTimeKindAttributeTests
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DateTimeKindAttributeTests()
        {
        }

        #endregion Constructor

        [Fact]
        public void DateTimeKindAttribute_Constructor_Default()
        {
            DateTimeKindAttribute converter = new DateTimeKindAttribute(DateTimeKind.Utc);
            Assert.True(converter.Kind == DateTimeKind.Utc);
        }

        [Fact]
        public void DateTimeKindAttribute_Apply_ParameterIsNull_DoesNothing()
        {
            DateTimeKindAttribute converter = new DateTimeKindAttribute(DateTimeKind.Utc);
            Assert.True(converter.Kind == DateTimeKind.Utc);

            DateTime dt = new DateTime(2018, 1, 1, 12, 30, 00, DateTimeKind.Local);
            DateTimeTestClass dateTimeTestClass = null;
            DateTimeKindAttribute.Apply(dateTimeTestClass);
        }

        [Fact]
        public void DateTimeKindAttribute_Apply_ParameterIsNotNull_DateTimeIsConverted()
        {
            DateTimeKindAttribute converter = new DateTimeKindAttribute(DateTimeKind.Utc);
            Assert.True(converter.Kind == DateTimeKind.Utc);

            DateTime dt = new DateTime(2018, 1, 1, 12, 30, 00, DateTimeKind.Local);
            DateTimeTestClass dateTimeTestClass = new DateTimeTestClass(dt);

            DateTimeKindAttribute.Apply(dateTimeTestClass);
            Assert.True(dateTimeTestClass.MyDateTime.Kind == DateTimeKind.Utc);
        }

        [Fact]
        public void DateTimeKindAttribute_Apply_ParameterIsNotNull_DateTimeIsNull_DoesNothing()
        {
            DateTimeKindAttribute converter = new DateTimeKindAttribute(DateTimeKind.Utc);
            Assert.True(converter.Kind == DateTimeKind.Utc);

            DateTime dt = new DateTime(2018, 1, 1, 12, 30, 00, DateTimeKind.Local);
            DateTimeTestClass dateTimeTestClass = new DateTimeTestClass(dt);

            DateTimeKindAttribute.Apply(dateTimeTestClass);        
        }

        private class DateTimeTestClass
        {
            [DateTimeKindAttribute(DateTimeKind.Utc)]
            public DateTime MyDateTime { get; private set; }

            [DateTimeKindAttribute(DateTimeKind.Utc)]
            public DateTime? MyNullableDateTime { get; private set; }

            public DateTimeTestClass(DateTime dt)
            {
                MyDateTime = dt;
            }
        }
    }
}