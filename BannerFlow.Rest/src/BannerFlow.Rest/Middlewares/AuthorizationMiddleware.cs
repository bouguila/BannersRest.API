using System.Threading.Tasks;
using BannerFlow.Rest.src.BannerFlow.Rest.Entities.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using MongoDB.Bson;

namespace BannerFlow.Rest.Entities.Extensions
{
    public class AuthorizationMiddleware
    {
       
        private readonly RequestDelegate _next;
        private readonly string _apiKey;

        public AuthorizationMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _apiKey = config.GetSection("AppSettings")["ApiKey"]; ;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.Keys.Contains("x-api-key"))
            {
                throw new UnauthorizedException("missing API Key");
            }

            if (!IsValidApiKey(context.Request.Headers["x-api-key"]))
            {
                throw new UnauthorizedException("Invalid API Key");
            }

            await _next.Invoke(context);
        }

        private bool IsValidApiKey(string apiKeyReqHeader)
        {
            return _apiKey == apiKeyReqHeader;
        }
    }

    #region ExtensionMethod
    public static class AuthorizationExtension
    {
        public static IApplicationBuilder ApplyAuthorization(this IApplicationBuilder app)
        {
            app.UseMiddleware<AuthorizationMiddleware>();
            return app;
        }
    }
    #endregion
}