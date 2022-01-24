using ApiDemo.Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiDemo.Api.Common.Dtos.Accounts.GetCustomerDetails
{
    public class CustomerDetailsDto
    {
        public int Id { get; init; }
        public int TitleId { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public IEnumerable<AddressDto> ResidentialAddresses { get; init; } = Array.Empty<AddressDto>();
        public IEnumerable<AddressDto> PostalAddresses { get; init; } = Array.Empty<AddressDto>();

        public Customer MapToCustomer()
        {
            return new Customer
            {
                Id = Id,
                Title = Title.From(TitleId),
                FirstName = FirstName,
                LastName = LastName,
                Addresses =
                    ResidentialAddresses
                        .Select(a => a.MapToAddress(AddressType.Residential))
                        .Concat(
                            PostalAddresses
                                .Select(a => a.MapToAddress(AddressType.Postal))
                        )
                        .ToArray()
            };
        }
    }
}
