using Fortin.Infrastructure.Implementation;
using Fortin.Infrastructure.Interface;
using Fortin.Proxy;

namespace Fortin.API.Configuration
{
    public static class ConfigurationCollectionExtension
    {
        public static IServiceCollection AddRepositoryCollection(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserEFRepository, UserEFRepository>();
            services.AddScoped<GitHubClient>();

            return services;
        }
    }
}
