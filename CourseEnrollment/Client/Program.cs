using CourseEnrollment.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CourseEnrollment.Client
{
    public class Program
    {
        //public static async Task Main(string[] args)
        //{
        //    var builder = WebAssemblyHostBuilder.CreateDefault(args);
        //    builder.RootComponents.Add<App>("#app");

        //    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        //    await builder.Build().RunAsync();




        //    builder.Services.AddHttpClient("CourseEnrollmentAPI", client =>
        //    {
        //        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
        //    })
        //    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

        //    builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("CourseEnrollmentAPI"));

        //}
        public static async Task Main(string[] args)
        {
            // Create the WebAssembly host builder
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            // Set the root component for the application
            builder.RootComponents.Add<App>("#app");

            // Configure services
            ConfigureServices(builder.Services, builder.HostEnvironment.BaseAddress);

            // Build and run the application
            var host = builder.Build();
            await host.RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services, string baseAddress)
        {
            // Register HttpClient with the base address
            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

            // Register other services
            services.AddScoped<AuthenticationService>();

        }
    }
}
