using ApiDemo.Api.Common;
using ApiDemo.Api.Domain.Entities;
using FluentValidation;
using System;
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

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.OrderId).GreaterThanOrEqualTo(1).WithMessage(c => $"{nameof(c.OrderId)} field must be greater than 1.");
            }
        }

        public class Handler : IRequestHandler<Command, OrderDto>
        {
            private readonly IGetCustomerDetailsHandler _getCustomerDetailsHandler;
            private readonly ICreatePackingOrderHandler _createPackingOrderHandler;
            private readonly IValidator<Command> _validator;

            public Handler(
                IGetCustomerDetailsHandler getCustomerDetailsHandler, 
                ICreatePackingOrderHandler createShippingHandler,
                IValidator<Command> validator)
            {
                _getCustomerDetailsHandler = getCustomerDetailsHandler;
                _createPackingOrderHandler = createShippingHandler;
                _validator = validator;
            }

            public async Task<OrderDto> Handle(Command message, CancellationToken token)
            {
                if (message is null)
                {
                    throw new ArgumentNullException(nameof(message));
                }

                await _validator.ValidateAndThrowAsync(message);

                // get customer identifier from orders database
                Order order = GetOrderFromRepository(message.OrderId);

                Customer customer = await _getCustomerDetailsHandler.Handle(order.CustomerId, token);

                order.Customer = customer;

                // this service endpoint would be idempotent in a production environment
                // which would allow for safe post retries on transient errors
                await _createPackingOrderHandler.Handle(order, token);

                return MapToOrderDto(order);
            }

            private static OrderDto MapToOrderDto(Order order)
            {
                if (order is null)
                {
                    throw new ArgumentNullException(nameof(order));
                }

                if (order.Customer is null)
                {
                    throw new ArgumentException($"{nameof(order.Customer)} cannot be null.");
                }

                var deliveryAddress = order.Customer.DeliveryAddress;

                return new OrderDto
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    CustomerFirstName = order.Customer.FirstName,
                    CustomerLastName = order.Customer.LastName,
                    CustomerTitle = order.Customer.Title,
                    DeliveryAddressLine1 = deliveryAddress.Line1,
                    DeliveryAddressLine2 = deliveryAddress.Line2,
                    DeliveryAddressSuburb = deliveryAddress.Suburb,
                    DeliveryAddressProvince = deliveryAddress.Province,
                    DelieryAddressPostalCode = deliveryAddress.PostalCode,
                    Items = 
                        order
                            .Items
                            .Select(i => new OrderItemDto 
                            { 
                                Id = i.Id,
                                ItemDescription = i.StockItem.Description,
                                ItemSize = i.StockItem.Size,
                                Quantity = i.Quantity
                            })
                            .ToArray(),
                };
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
                            StockItem = new StockItem
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
                            StockItem = new StockItem
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
        }

    }
}
