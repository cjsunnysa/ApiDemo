using Polly;
using System;
using System.Net.Http;

namespace ApiDemo.Api.Infrastructure.Services
{
    public class TimeoutPolicy
    {
        public record Config
        {
            public int Seconds { get; init; }
        }

        public static IAsyncPolicy<HttpResponseMessage> Create(Config config)
        {
            return
                config is null ? throw new ArgumentNullException(nameof(config)) :
                    config.Seconds == default ? Policy.NoOpAsync<HttpResponseMessage>() :
                        Policy.TimeoutAsync<HttpResponseMessage>(config.Seconds);
        }

    }
}
