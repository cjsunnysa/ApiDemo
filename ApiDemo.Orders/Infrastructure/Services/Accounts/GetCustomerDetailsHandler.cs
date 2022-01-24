using ApiDemo.Api.Common;
using ApiDemo.Api.Common.Dtos.Accounts.GetCustomerDetails;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ApiDemo.Api.Infrastructure.Services
{
    public class GetCustomerDetailsHandler : HttpClientService, IGetCustomerDetailsHandler
    {
        public GetCustomerDetailsHandler(HttpClient httpClient)
            : base(httpClient)
        { }

        public async Task<CustomerDetailsDto> Handle(int id, CancellationToken cancellationToken)
        {
            return await Get<CustomerDetailsDto>($"api/customers/{id}", cancellationToken);
        }
    }
}
