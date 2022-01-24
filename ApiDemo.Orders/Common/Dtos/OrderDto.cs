using System;
using System.Collections.Generic;

namespace ApiDemo.Api.Common
{
    public class OrderDto
    {
        public int Id { get; init; }
        public int CustomerId { get; init; }
        public string CustomerTitle { get; init; }
        public string CustomerFirstName { get; init; }
        public string CustomerLastName { get; init; }
        public string DeliveryAddressLine1 { get; init; }
        public string DeliveryAddressLine2 { get; init; }
        public string DeliveryAddressSuburb { get; init; }
        public int DelieryAddressPostalCode { get; init; }
        public string DeliveryAddressProvince { get; init; }
        public IEnumerable<OrderItemDto> Items { get; init; } = Array.Empty<OrderItemDto>();
    }
}
