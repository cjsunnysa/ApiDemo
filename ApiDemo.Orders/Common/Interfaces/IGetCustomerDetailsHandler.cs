using ApiDemo.Api.Domain.Entities;

namespace ApiDemo.Api.Common
{
    public interface IGetCustomerDetailsHandler : IRequestHandler<int, Customer>
    { }
}
