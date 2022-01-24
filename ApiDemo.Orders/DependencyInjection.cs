using ApiDemo.Api.Common;
using ApiDemo.Api.Infrastructure.Services;
using ApiDemo.Api.Infrastructure.Services.Shipping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Net.Http;

namespace ApiDemo.Api
{

    public static class DependencyInjection
    {
        public static IServiceCollection AddFeatures(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<Features.ConfirmOrder.Command, OrderDto>, Features.ConfirmOrder.Handler>();

            return services;
        }

        public static IServiceCollection AddInfrasructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClientForGetCustomerDetails(configuration);
            services.AddHttpClientForCreateShipping(configuration);

            return services;
        }

        private static IServiceCollection AddHttpClientForGetCustomerDetails(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration.GetValue<string>($"Services:Accounts:BaseUrl");

            services
                .AddHttpClient<IGetCustomerDetailsHandler, GetCustomerDetails.Handler>(ConfigureHttpClient(baseUrl))
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(CreateRetryPolicy("Accounts:Endpoints:GetCustomerDetails", configuration))
                .AddPolicyHandler(CreateTimeoutPolicy("Accounts:Endpoints:GetCustomerDetails", configuration));

            return services;
        }

        private static IServiceCollection AddHttpClientForCreateShipping(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration.GetValue<string>($"Services:Shipping:BaseUrl");

            services
                .AddHttpClient<ICreatePackingOrderHandler, CreatePackingOrder.Handler>(ConfigureHttpClient(baseUrl))
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(CreateRetryPolicy("Shipping:Endpoints:Create", configuration))
                .AddPolicyHandler(CreateTimeoutPolicy("Shipping:Endpoints:Create", configuration));

            return services;
        }


        private static Action<HttpClient> ConfigureHttpClient(string baseUrl)
        {
            return client => client.BaseAddress = new Uri(baseUrl);
        }

        private static IAsyncPolicy<HttpResponseMessage> CreateRetryPolicy(string serviceName, IConfiguration configuration)
        {
            var configurationSection = $"Services:{serviceName}:RetryOnTransientError";

            var config = new RetryPolicy.Config
            {
                IsEnabled = configuration.GetValue<bool>($"{configurationSection}:Enabled"),
                NumberOfRetries = configuration.GetValue<int>($"{configurationSection}:NumberOfRetries"),
                UseExponentialWait = configuration.GetValue<bool>($"{configurationSection}:UseExponentialWait"),
                WaitTimeSeconds = configuration.GetValue<int>($"{configurationSection}:WaitTimeSeconds")
            };

            return RetryPolicy.Create(config);
        }

        private static IAsyncPolicy<HttpResponseMessage> CreateTimeoutPolicy(string serviceName, IConfiguration configuration)
        {
            var timeout = configuration.GetValue<int>($"Services:{serviceName}:TimeoutSeconds");

            var config = new TimeoutPolicy.Config
            {
                Seconds = timeout
            };

            return TimeoutPolicy.Create(config);
        }
    }
}
