using ApiDemo.Api.Common;
using ApiDemo.Api.Common.Commands;
using ApiDemo.Api.Common.Dtos.Accounts.GetCustomerDetails;
using ApiDemo.Api.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiDemo.Api.Features
{
    public class ConfirmOrder
    {
        public record Command
        {
            public int OrderId { get; init; }
        }

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IGetCustomerDetailsHandler _getCustomerDetailsHandler;
            private readonly ICreatePackingOrderHandler _createPackingOrderHandler;

            public Handler(
                IGetCustomerDetailsHandler getCustomerDetailsHandler, 
                ICreatePackingOrderHandler createShippingHandler)
            {
                _getCustomerDetailsHandler = getCustomerDetailsHandler;
                _createPackingOrderHandler = createShippingHandler;
            }

            public async Task<bool> Handle(Command message, CancellationToken token)
            {
                // get customer identifier from orders database
                Order order = GetOrderFromRepository(message.OrderId);

                CustomerDetailsDto customerDetails = await _getCustomerDetailsHandler.Handle(order.CustomerId, token);

                Customer customer = customerDetails.MapToCustomer();

                CreatePackingOrderCommand command = MapToCreatePackingOrderCommand(customer, order);

                // this service endpoint would be idempotent in a production environment
                // which would allow for safe post retries on transient errors
                await _createPackingOrderHandler.Handle(command, token);

                return true;
            }

            private static Order GetOrderFromRepository(int orderId)
            {
                return new Order
                {
                    Id = orderId,
                    CustomerId = 1,
                    Items = new[]
                                    {
                        new OrderItem
                        {
                            Id = 694939292,
                            Quantity = 2,
                            Item = new StockItem
                            {
                                Id = 320030218,
                                Sku = "1920000000008920",
                                Description = "Jalepeno Seeds",
                                Size = "20 pack"
                            }
                        },
                        new OrderItem
                        {
                            Id = 694939293,
                            Quantity = 12,
                            Item = new StockItem
                            {
                                Id = 320030218,
                                Sku = "1920000000002914",
                                Description = "Carolina Reaper Seeds",
                                Size = "Single"
                            }
                        },
                    }
                };
            }

            private static CreatePackingOrderCommand MapToCreatePackingOrderCommand(Customer customer, Order order)
            {
                Address deliveryAddress = customer.DeliveryAddress;

                return new CreatePackingOrderCommand
                {
                    CustomerName = customer.FullName,
                    DeliveryAddressLine1 = deliveryAddress.Line1,
                    DeliveryAddressLine2 = deliveryAddress.Line2,
                    DeliveryAddressSuburb = deliveryAddress.Suburb,
                    DeliveryAddressPostalCode = deliveryAddress.PostalCode,
                    DeliveryAddressProvince = deliveryAddress.Province.Name,
                    Items = 
                        order
                            .Items
                            .Select(item => new CreatePackingOrderCommand.PackingOrderItem(item))
                            .ToArray()
                };
            }
        }

    }
}
