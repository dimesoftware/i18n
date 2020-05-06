using System.Linq;
using Xunit;

namespace System.Globalization.Countries.Tests
{
    public class CountryListTests
    {
        [Fact]
        public void CountryList_Count_ShouldReturn239()
        {
            CountryList countries = new CountryList();
            Assert.True(countries.Count() == 239);
        }

        [Fact]
        public void CountryList_Distinct_Count_ShouldReturn239()
        {
            CountryList countries = new CountryList();
            Assert.True(countries.Distinct().Count() == 239);
        }
    }
}