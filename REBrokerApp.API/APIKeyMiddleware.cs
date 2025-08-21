using REBrokerApp.Infrastructure;
using System.Net;

namespace REBrokerApp.API
{
    public class APIKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string API_KEY_HEADER_NAME = "X-API-Key";

        public APIKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            // Skip validation for Swagger/development endpoints
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKey))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("API Key is missing.");
                return;
            }

            var apiKey = configuration["ApiKey"];
            if (!string.Equals(apiKey, extractedApiKey))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid API Key.");
                return;
            }

            // API Key is valid — continue processing
            await _next(context);
        }
    }
}