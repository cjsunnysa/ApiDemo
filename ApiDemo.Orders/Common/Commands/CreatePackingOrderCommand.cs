using ApiDemo.Api.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ApiDemo.Api.Common.Commands
{
    public class CreatePackingOrderCommand
    {
        public string CustomerName { get; init; }
        public string DeliveryAddressLine1 { get; init; }
        public string DeliveryAddressLine2 { get; init; }
        public string DeliveryAddressSuburb { get; init; }
        public string DeliveryAddressProvince { get; init; }
        public int DeliveryAddressPostalCode { get; init; }
        public IEnumerable<PackingOrderItem> Items { get; init; }

        public class PackingOrderItem
        {
            public PackingOrderItem(OrderItem orderItem)
            {
                if (orderItem is null)
                {
                    throw new ArgumentNullException(nameof(orderItem));
                }

                if (orderItem.Item is null)
                {
                    throw new ArgumentNullException("Order item does not contain a stock item.");
                }

                Sku = orderItem.Item.Sku;
                ProductName = $"{orderItem.Item.Description} {orderItem.Item.Size}";
                Quanity = orderItem.Quantity;
            }

            public string Sku { get; init; }
            public string ProductName { get; init; }
            public double Quanity { get; init; }
        }
    }
}
