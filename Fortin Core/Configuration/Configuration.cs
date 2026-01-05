using Fortin.Domain.Implementation;
using Fortin.Domain.Interface;
using Fortin.Infrastructure.Implementation;
using Fortin.Infrastructure.Interface;
using Fortin.Infrastructure.Services;
using Fortin.Proxy;

namespace Fortin.API.Configuration
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserEFRepository, UserEFRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductModelRepository, ProductModelRepository>();

            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }

        public static IServiceCollection AddProxyServices(this IServiceCollection services)
        {
            services.AddHttpClient<HttpProxy>();
            services.AddScoped<GitHubClient>();
            return services;
        }
    }
}
