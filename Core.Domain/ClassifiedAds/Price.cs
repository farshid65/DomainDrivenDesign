using Core.Domain.Shared;

namespace Core.Domain.ClassifiedAds
{
    public class Price : Money
    {
        protected Price() { }
        private Price(decimal amount,
           string currencyCode,
           ICurrencyLookup currencyLookup) : base(amount, currencyCode, currencyLookup)
        {
            if (amount < 0) throw new ArgumentException("Price cannot be negative", nameof(amount));
        }
        internal Price(decimal amount, string currencyCode)
  : base(amount, new Currency
  {
      CurrencyCode =
  currencyCode
  })
        { }
        public static Price FromDecimal(decimal amount, string currency, ICurrencyLookup currencyLookup) =>
            new Price(amount, currency, currencyLookup);
        public static Price NoPrice =
            new Price();

    }

}