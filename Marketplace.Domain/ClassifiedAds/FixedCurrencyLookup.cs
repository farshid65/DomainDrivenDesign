using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marketplace.Domain.Shared;

namespace Marketplace.Domain.ClassifiedAds
{
    public class FixedCurrencyLookup : ICurrencyLookup
    {
        private static readonly IEnumerable<Currency> _currencies =
            new[]
            {
                new Currency
                {
                    CurrencyCode="Eur",
                    DecimalPlaces=2,
                    InUse=true
                },
                new Currency
                {
                    CurrencyCode="USD",
                    DecimalPlaces=2,
                    InUse=true
                }

            };
        public Currency FindCurrency(string currencyCode)
        {
            var currency = _currencies.FirstOrDefault(c => c.CurrencyCode == currencyCode);
            return currency ?? Currency.None;
        }
    }
}
