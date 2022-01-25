using ApiDemo.Api.Common;
using ApiDemo.Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ApiDemo.Api.Infrastructure.Services
{
    public class GetCustomerDetails
    {
        private class CustomerDetailsDto
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

        private class AddressDto
        {
            public int Id { get; init; }
            public string Line1 { get; init; }
            public string Line2 { get; init; } = string.Empty;
            public string Suburb { get; init; }
            public int ProvinceId { get; init; }
            public int PostalCode { get; init; }
            public bool IsPrimary { get; init; }
            public bool IsActive { get; init; }
            public DateTime CreatedDate { get; init; }

            public Address MapToAddress(AddressType addressType)
            {
                return new Address
                {
                    Id = Id,
                    Type = addressType,
                    Line1 = Line1,
                    Line2 = Line2,
                    Suburb = Suburb,
                    Province = Province.From(ProvinceId),
                    PostalCode = PostalCode,
                    IsPrimary = IsPrimary,
                    IsActive = IsActive,
                    CreatedDate = CreatedDate
                };
            }
        }

        public class Handler : HttpClientService, IGetCustomerDetailsHandler
        {
            public Handler(HttpClient httpClient)
                : base(httpClient)
            { }

            public async Task<Customer> Handle(int id, CancellationToken cancellationToken)
            {
                CustomerDetailsDto customerDetails = await Get<CustomerDetailsDto>($"api/customers/{id}", cancellationToken);

                return customerDetails.MapToCustomer();
            }
        }
    }

}
