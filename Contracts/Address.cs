namespace Contracts
{
    public class Address
    {
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
    }
    public class UpdateCustomerAddressDetails
    {
        public string? BillingStreet { get; set; }
        public string? BillingCity { get; set; }
        public string? BillingPostalCode { get; set; }
        public string? BillingCountry { get; set; }
        public string? DeliveryStreet { get; set; }
        public string? DeliveryCity { get; set; }
        public string? DeliveryPostalCode { get; set; }
        public string? DeliveryCountry { get; set; }
        public Address? DeliveryAddress { get;set; }
        public Address? BillingAddress { get; set; }

    }
}