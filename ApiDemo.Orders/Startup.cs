using ApiDemo.Api.Features;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApiDemo.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddInfrasructure(Configuration);

            services.AddFeatures();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddTransient<IValidator<ConfirmOrder.Command>, ConfirmOrder.Validator>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var errorPath = env.IsDevelopment() ? "/error-dev" : "/error";
            
            app.UseExceptionHandler(errorPath);
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
