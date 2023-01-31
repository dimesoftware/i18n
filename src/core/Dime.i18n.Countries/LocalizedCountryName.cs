using System.Numerics;
using System.Runtime.CompilerServices;

namespace Dime.i18n.Countries.Nations
{
    public class LocalizedCountryName
    {
        public LocalizedCountryName(string locale, string val)
        {
            Locale = locale;
            Value = val;
        }

        public string Locale { get; set; }
        public string Value { get; set; }

        public static implicit operator string(LocalizedCountryName localizedCountryName) => localizedCountryName.Value;
    }
}