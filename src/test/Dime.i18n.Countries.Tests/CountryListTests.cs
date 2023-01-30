using System.Collections;
using System.Linq;
using Xunit;

namespace System.Globalization.Countries.Tests
{
    public class CountryListTests
    {
        [Fact]
        public void CountryList_Count_ShouldReturn239()
        {
            NationsList countries = new();
            Assert.True(countries.Count() == 247);
        }

        [Fact]
        public void CountryList_Distinct_Count_ShouldReturn247()
        {
            NationsList countries = new();
            Assert.True(countries.Distinct().Count() == 247);
        }

        [Fact]
        public void CountryList_GetEnumerator_ShouldEnumerate()
        {
            NationsList countries = new();

            IEnumerator enumerator = (countries as IEnumerable).GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (string.IsNullOrEmpty(enumerator.Current as string))
                    throw new Exception();
            }

            Assert.True(countries.Distinct().Count() == 247);
        }
    }
}