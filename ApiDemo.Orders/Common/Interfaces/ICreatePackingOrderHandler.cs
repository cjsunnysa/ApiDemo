using ApiDemo.Api.Domain.Entities;

namespace ApiDemo.Api.Common
{
    public interface ICreatePackingOrderHandler : IRequestHandler<Order, bool>
    { }

}
