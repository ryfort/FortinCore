using Fortin.API.Middlewares;
namespace Fortin.API.Configuration
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<TransactionMiddleware>();

            return app;
        }
    }
}
