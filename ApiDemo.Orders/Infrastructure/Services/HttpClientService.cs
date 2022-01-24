using ApiDemo.Api.Common;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ApiDemo.Api.Infrastructure.Services
{
    public abstract class HttpClientService
    {
        protected readonly HttpClient _httpClient;

        public HttpClientService(HttpClient client)
        {
            _httpClient = client;
        }

        protected async Task<TResponse> Get<TResponse>(string url, CancellationToken cancellationToken)
        {
            var body = await _httpClient.GetStringAsync(url, cancellationToken);

            return JsonConvert.DeserializeObject<TResponse>(body);
        }

        protected async Task<TResponse> Post<TResponse>(string url, HttpContent content, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsync(url, content, cancellationToken);

            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            return JsonConvert.DeserializeObject<TResponse>(body);
        }

        protected async Task<HttpResponseMessage> Post(string url, HttpContent content, CancellationToken cancellationToken)
        {
            return await _httpClient.PostAsync(url, content, cancellationToken);
        }

        protected void SetRequestHeader(string key, string value)
        {
            _httpClient.DefaultRequestHeaders.Add(key, value);
        }
    }
}
