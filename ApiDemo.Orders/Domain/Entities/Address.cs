using System;

namespace ApiDemo.Api.Domain.Entities
{
    public class Address
    {
        public int Id { get; init; }
        public AddressType Type { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; } = string.Empty;
        public string Suburb { get; set; }
        public Province Province { get; set; }
        public int PostalCode { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; init; }

        public bool IsResidentialAddress => Type == AddressType.Residential;

        public bool IsPostalAddress => Type == AddressType.Postal;
    }
}
