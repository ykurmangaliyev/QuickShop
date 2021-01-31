using Logging;
using Microsoft.Extensions.DependencyInjection;
using QuickShop.Domain.Accounts;
using QuickShop.Domain.Accounts.Authentication;
using QuickShop.Domain.Accounts.Authentication.HashingAlgorithm;
using QuickShop.Domain.Accounts.Model.UserAggregate;
using QuickShop.Domain.Ping;

namespace QuickShop.DependencyInjection
{
    public static class QuickShopServiceCollectionExtensions
    {
        public static IQuickShopServiceBuilder AddQuickShop(this IServiceCollection services)
        {
            // Accounts
            services.AddSingleton<IHashingAlgorithm, Sha512HashingAlgorithm>();

            services.AddTransient<IUserAuthService, UserAuthService>();

            services.AddTransient<IUserRepository, UserRepository>();

            // Ping
            services.AddSingleton<IPingService, PingService>();

            // Logging
            services.AddSingleton<ILogger, ConsoleLogger>();

            // Return builder
            return new QuickShopServiceBuilder(services);
        }
    }
}