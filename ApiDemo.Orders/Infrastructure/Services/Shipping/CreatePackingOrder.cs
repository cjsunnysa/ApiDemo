﻿using ApiDemo.Api.Common;
using ApiDemo.Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ApiDemo.Api.Infrastructure.Services.Shipping
{
    public class CreatePackingOrder
    {
        public class CreatePackingOrderCommand
        {
            public CreatePackingOrderCommand(Order order)
            {
                if (order is null)
                {
                    throw new ArgumentNullException(nameof(order));
                }

                if (order.Customer is null)
                {
                    throw new ArgumentException($"{nameof(order.Customer)} cannot be null.");
                }

                if (!order.Items.Any())
                {
                    throw new ArgumentException($"{nameof(order.Items)} cannot be empty.");
                }

                Address deliveryAddress = order.Customer.DeliveryAddress;

                CustomerName = order.Customer.FullName;
                DeliveryAddressLine1 = deliveryAddress.Line1;
                DeliveryAddressLine2 = deliveryAddress.Line2;
                DeliveryAddressSuburb = deliveryAddress.Suburb;
                DeliveryAddressPostalCode = deliveryAddress.PostalCode;
                DeliveryAddressProvince = deliveryAddress.Province.Name;
                Items =
                    order
                        .Items
                        .Select(item => new PackingOrderItem(item))
                        .ToArray();
            }

            public string CustomerName { get; init; }
            public string DeliveryAddressLine1 { get; init; }
            public string DeliveryAddressLine2 { get; init; }
            public string DeliveryAddressSuburb { get; init; }
            public string DeliveryAddressProvince { get; init; }
            public int DeliveryAddressPostalCode { get; init; }
            public IEnumerable<PackingOrderItem> Items { get; init; }
        }

        public class PackingOrderItem
        {
            public PackingOrderItem(OrderItem orderItem)
            {
                if (orderItem is null)
                {
                    throw new ArgumentNullException(nameof(orderItem));
                }

                if (orderItem.StockItem is null)
                {
                    throw new ArgumentException($"{nameof(orderItem.StockItem)} cannot be null.");
                }

                Sku = orderItem.StockItem.Sku;
                ProductName = $"{orderItem.StockItem.Description} {orderItem.StockItem.Size}";
                Quanity = orderItem.Quantity;
            }

            public string Sku { get; init; }
            public string ProductName { get; init; }
            public double Quanity { get; init; }
        }
        
        public class Handler : HttpClientService, ICreatePackingOrderHandler
        {
            public Handler(HttpClient httpClient)
                : base(httpClient)
            { }

            public async Task<bool> Handle(Order order, CancellationToken token)
            {
                if (order is null)
                {
                    throw new ArgumentNullException(nameof(order));
                }

                SetRequestHeader("Idempotency-key", Guid.NewGuid().ToString());

                CreatePackingOrderCommand command = new(order);

                string serializedRequest = JsonSerializer.Serialize(command);

                StringContent content = new(serializedRequest, Encoding.UTF8, "application/json");

                await Post("api/packing-orders", content, token);

                return true;
            }
        }
    }

}
