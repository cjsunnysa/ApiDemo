using ApiDemo.Api.Common;
using ApiDemo.Api.Common.Commands;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApiDemo.Api.Infrastructure.Services.Shipping
{
    public class CreatePackingOrderHandler : HttpClientService, ICreatePackingOrderHandler
    {
        public CreatePackingOrderHandler(HttpClient httpClient)
            : base(httpClient)
        { }

        public async Task<bool> Handle(CreatePackingOrderCommand command, CancellationToken token)
        {
            SetRequestHeader("Idempotency-key", Guid.NewGuid().ToString());

            var serializedRequest = JsonConvert.SerializeObject(command);
            
            var content = new StringContent(serializedRequest, Encoding.UTF8, "application/json");

            await Post("api/packing-orders", content, token);

            return true;
        }
    }
}
