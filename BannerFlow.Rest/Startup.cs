using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BannerFlow.Rest.Contracts;
using BannerFlow.Rest.Entities.Extensions;
using BannerFlow.Rest.Services;
using BannerFlow.Rest.src.BannerFlow.Rest.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace BannerFlow.Rest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigin", 
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyHeader());
            });

            services.AddScoped<IBannerService, BannerService>()
                    .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            #region
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            #endregion

            app.UseCors("AllowAllOrigin")
               .ApplyExceptionHandling()
               .ApplyAuthorization()
               .UseHttpsRedirection()
               .UseMvc();
        }
    }
}
