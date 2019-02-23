using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using MongoDB.Bson;

namespace BannerFlow.Rest.Entities.Extensions
{
    public class ApiKeyValidatorMiddleware
    {
       
        private readonly RequestDelegate _next;
        private readonly string _apiKey;

        public ApiKeyValidatorMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _apiKey = config.GetSection("AppSettings")["ApiKey"]; ;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.Keys.Contains("x-api-key"))
            {
                context.Response.StatusCode = 400; //Bad Request                
                await context.Response.WriteAsync("API Key is missing");
                return;
            }
            else
            {
                if (!IsValidApiKey(context.Request.Headers["x-api-key"]))
                {
                    context.Response.StatusCode = 401; //UnAuthorized              
                    await context.Response.WriteAsync("Invalid API Key");
                    return;
                }
            }

            await _next.Invoke(context);
        }

        private bool IsValidApiKey(string apiKeyReqHeader)
        {
            return _apiKey == apiKeyReqHeader;
        }
    }

    #region ExtensionMethod
    public static class ApiKeyValidatorsExtension
    {
        public static IApplicationBuilder ApplyApiKeyValidation(this IApplicationBuilder app)
        {
            app.UseMiddleware<ApiKeyValidatorMiddleware>();
            return app;
        }
    }
    #endregion
}