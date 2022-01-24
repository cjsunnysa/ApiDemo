
using ApiDemo.Api.Common.Dtos.Accounts.GetCustomerDetails;

namespace ApiDemo.Api.Common
{
    public interface IGetCustomerDetailsHandler : IRequestHandler<int, CustomerDetailsDto>
    { }
}
