using Polly;
using Polly.Timeout;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace ApiDemo.Api.Infrastructure.Services
{
    public class RetryPolicy
    {
        private static readonly HttpStatusCode[] _responseCodesWorthRetry =
        {
            HttpStatusCode.RequestTimeout,
            HttpStatusCode.InternalServerError,
            HttpStatusCode.BadGateway,
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.GatewayTimeout
        };

        public record Config
        {
            public bool IsEnabled { get; init; }
            public int NumberOfRetries { get; init; }
            public bool UseExponentialWait { get; init; }
            public int WaitTimeSeconds { get; init; }
        }

        public static IAsyncPolicy<HttpResponseMessage> Create(Config config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (!config.IsEnabled)
            {
                return Policy.NoOpAsync<HttpResponseMessage>();
            }

            return
                Policy
                    .Handle<HttpRequestException>()
                    .Or<TimeoutRejectedException>()
                    .OrResult<HttpResponseMessage>(ex => _responseCodesWorthRetry.Contains(ex.StatusCode))
                    .WaitAndRetryAsync(config.NumberOfRetries, ConfigureRetryWait(config));
        }

        private static Func<int, TimeSpan> ConfigureRetryWait(Config config)
        {
            Func<int, double> waitSeconds =
                attempt =>
                    config.UseExponentialWait
                    ? Math.Pow(config.WaitTimeSeconds, attempt)
                    : config.WaitTimeSeconds;

            return attempt => TimeSpan.FromSeconds(waitSeconds(attempt));
        }

    }
}
