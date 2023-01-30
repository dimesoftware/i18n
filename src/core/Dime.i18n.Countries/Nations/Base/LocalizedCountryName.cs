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
    }
}