using System;
using System.Collections.Generic;

namespace ApiDemo.Api.Domain.Entities
{
    public class Order
    {
        public int Id { get; init; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public IEnumerable<OrderItem> Items { get; set; } = Array.Empty<OrderItem>();
    }
}
