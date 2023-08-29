namespace Marketplace.Domain.ClassifiedAds
{
    public class CurrencyMismatchException : Exception
    {
        public CurrencyMismatchException(string message) : base(message)
        {
        }
    }
}



