using ApiDemo.Api.Domain.Entities;
using System;

namespace ApiDemo.Api.Common.Dtos.Accounts.GetCustomerDetails
{
    public class AddressDto
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
}
