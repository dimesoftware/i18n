using System.Collections.Generic;
using System.Linq;

namespace Dime.i18n.Countries.Nations
{
    public abstract class Nation
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string AltCode { get; set; }        
        public List<LocalizedCountryName> Locales { get; set; } = new List<LocalizedCountryName>();

        protected void Translate(string locale, string val) => Locales.Add(new(locale, val));

        public string this[string i] => Locales.FirstOrDefault(x => x.Locale == i)?.Value;
    }
}