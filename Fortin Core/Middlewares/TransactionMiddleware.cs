using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Fortin.API.Middlewares
{
    public class TransactionMiddleware
    {
        private readonly RequestDelegate _next;
        public TransactionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var apiKey = context.Request.Query["api_key"];

            if (string.IsNullOrEmpty(apiKey))
            {
                await context.Response.WriteAsync("API Key is required.");
                return;
            }

            context.Response.Headers.Append("X-Transaction-Id", Guid.NewGuid().ToString());

            await _next(context);
        }
    }
}
