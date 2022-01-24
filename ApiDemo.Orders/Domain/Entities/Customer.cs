using ApiDemo.Api.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiDemo.Api.Domain.Entities
{
    public class Customer
    {
        public int Id { get; init; }
        public Title Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<Address> Addresses { get; set; } = Array.Empty<Address>();
        public string FullName => $"{Title.Description} {FirstName} {LastName}";
        
        // order of address usage:
        // 1. primary postal address
        // 2. primary residential address
        // 3. last created active residential address
        // 4. last created active postal address
        // 5. last created residential address
        // 6. last created postal address
        public Address DeliveryAddress =>
            Addresses.FirstOrDefault(a => a.IsPostalAddress && a.IsPrimary) ??
            Addresses.FirstOrDefault(a => a.IsResidentialAddress && a.IsPrimary) ??
            LastCreatedAddress ??
            throw new InvalidDeliveryAddressException($"{FirstName} {LastName}", Id);
        
        private Address LastCreatedAddress =>
            Addresses
                .OrderByDescending(a => a.CreatedDate)
                .FirstOrDefault(a =>
                    (a.IsResidentialAddress && a.IsActive) ||
                    (a.IsPostalAddress && a.IsActive) ||
                    a.IsResidentialAddress || 
                    a.IsPostalAddress
                );
    }
}
