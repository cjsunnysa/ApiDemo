using ApiDemo.Api.Common;
using ApiDemo.Api.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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

                Sku = orderItem.Item.Sku;
                ProductName = $"{orderItem.Item.Description} {orderItem.Item.Size}";
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
                SetRequestHeader("Idempotency-key", Guid.NewGuid().ToString());

                var command = new CreatePackingOrderCommand(order);

                var serializedRequest = JsonConvert.SerializeObject(command);
            
                var content = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

                await Post("api/packing-orders", content, token);

                return true;
            }
        }
    }

}
