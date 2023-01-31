using System.Collections;
using System.Linq;
using Dime.i18n.Countries.Nations;
using Xunit;

namespace System.Globalization.Countries.Tests
{
    public class CountryListTests
    {
        [Fact]
        public void CountryList_Count_ShouldReturn247()
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
                if (enumerator.Current == null)
                    throw new Exception();
            }

            Assert.True(countries.Distinct().Count() == 247);
        }


        [Fact]
        public void Country_GetTranslations_ShouldEnumerate()
        {
            NationsList countries = new();
            Nation belgium = countries["be"];

            Assert.True(belgium.Code == "BE");
            Assert.True(belgium.AltCode == "BEL");
            Assert.True(belgium["nl"] == "België");
        }
    }
}