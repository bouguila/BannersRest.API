using System;
using System.Collections.Generic;
using BannerFlow.Rest.Contracts;
using BannerFlow.Rest.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace BannerFlow.Rest.IntegrationTests
{
    public class TestWebAppFactory<TStartup> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<IBannerService, BannerService>()
                    .AddMvcCore().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            });
        }
    }
}
