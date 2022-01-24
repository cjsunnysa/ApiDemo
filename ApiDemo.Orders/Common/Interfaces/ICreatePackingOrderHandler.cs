using ApiDemo.Api.Common.Commands;

namespace ApiDemo.Api.Common
{
    public interface ICreatePackingOrderHandler : IRequestHandler<CreatePackingOrderCommand, bool>
    { }

}
