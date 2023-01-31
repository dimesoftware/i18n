using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace Dime.i18n.Countries.Nations
{
    public class Nation
    {
        public Nation(string code, string altCode, Type resx)
        {
            Code = code;
            AltCode = altCode;
            Resx = resx;
        }

        public string Code { get; set; }
        public string AltCode { get; set; }

        private Type Resx { get; set; }

        public IEnumerable<LocalizedCountryName> Locales
        {
            get
            {
                if (Resx == null)
                    yield break;

                ResourceManager rm = new(Resx);
                ResourceSet resourceSet = rm?.GetResourceSet(CultureInfo.InvariantCulture, true, true);
                if (resourceSet == null)
                    yield break;

                foreach (DictionaryEntry entry in resourceSet)
                {
                    string resourceKey = entry.Key.ToString();
                    object resource = entry.Value;

                    yield return new LocalizedCountryName(resourceKey, resource.ToString());
                }
            }
        }

        public string this[string i] => Locales.FirstOrDefault(x => x.Locale.Equals(i, StringComparison.OrdinalIgnoreCase))?.Value;
    }
}