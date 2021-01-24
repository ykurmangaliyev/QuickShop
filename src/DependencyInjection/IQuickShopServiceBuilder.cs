using Microsoft.Extensions.DependencyInjection;

namespace QuickShop.DependencyInjection
{
    public interface IQuickShopServiceBuilder
    {
        IServiceCollection Services { get; }
    }
}