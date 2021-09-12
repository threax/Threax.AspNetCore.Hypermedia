using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threax.AspNetCore.Models
{
    public class CurrencyUiOptions : PropertyUiInfo
    {
        public CurrencyUiOptions(String locale, String currency, string type) : base(type)
        {
            Locale = locale;
            Currency = currency;
        }

        public override string CreateAttribute()
        {
            var sb = new StringBuilder();
            sb.Append("[CurrencyUiType(");
            bool needComma = false;
            if (Locale != null)
            {
                sb.Append($"locale: {Locale}");
                needComma = true;
            }
            if (Currency != null)
            {
                if (needComma)
                {
                    sb.Append(", ");
                }
                sb.Append($"currency: {DateTimeUiTypeAttribute.Timezones.GetTimezoneCodeName(Currency)}");
            }
            sb.Append($"{AddSharedProperties(needComma)})]");
            return sb.ToString();
        }

        public string Locale { get; }
        public string Currency { get; }
    }

    /// <summary>
    /// Use this to change the ui type of a property to a time. This will
    /// cause uis to allow just time input instead of date and time.
    /// </summary>
    public class CurrencyUiTypeAttribute : UiTypeAttribute
    {
        public const String UiName = "currency";

        /// <summary>
        /// Create a currency with the given locale and currency.
        /// </summary>
        /// <param name="locale">The locale to use. Default: 'en-US'</param>
        /// <param name="currency">The currency to use. Default: 'USD'</param>
        public CurrencyUiTypeAttribute(String locale = "en-US", String currency = "USD") : base(new CurrencyUiOptions(locale, currency, UiName))
        {
        }
    }
}
