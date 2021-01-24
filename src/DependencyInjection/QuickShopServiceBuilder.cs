using Microsoft.Extensions.DependencyInjection;

namespace QuickShop.DependencyInjection
{
    internal class QuickShopServiceBuilder : IQuickShopServiceBuilder
    {
        public QuickShopServiceBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
