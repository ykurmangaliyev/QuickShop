using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using QuickShop.Domain.Accounts.Model.UserAggregate;
using QuickShop.PaymentProvider.Abstractions;
using QuickShop.PaymentProvider.Stripe;
using QuickShop.PaymentProvider.Stripe.Configuration;
using QuickShop.Repository.Abstractions;
using QuickShop.Repository.Mongo;
using QuickShop.Repository.Mongo.CollectionNameMapping;
using QuickShop.Repository.Mongo.Configuration;

namespace QuickShop.DependencyInjection.PaymentProvider
{
    public static class StripePaymentProviderExtensions
    {
        public static IQuickShopServiceBuilder AddStripePaymentProvider(this IQuickShopServiceBuilder builder)
            => AddStripePaymentProvider(builder, _ => { });

        public static IQuickShopServiceBuilder AddStripePaymentProvider(this IQuickShopServiceBuilder builder, Action<StripeOptions> options)
        {
            if (builder == null) 
                throw new ArgumentNullException(nameof(builder));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            builder.Services.Configure(options);

            builder.Services.AddSingleton<IPaymentProvider, StripePaymentProvider>();

            return builder;
        }

        public static IServiceCollection ConfigureStripePaymentProviderOptions(this IServiceCollection services, IConfiguration configuration) 
            => ConfigureStripePaymentProviderOptions(services, configuration, StripeOptions.Stripe);

        public static IServiceCollection ConfigureStripePaymentProviderOptions(this IServiceCollection services, IConfiguration configuration, string sectionName)
        {
            if (services == null) 
                throw new ArgumentNullException(nameof(services));

            if (configuration == null) 
                throw new ArgumentNullException(nameof(configuration));

            if (string.IsNullOrWhiteSpace(sectionName)) 
                throw new ArgumentException(nameof(sectionName));

            services.Configure<MongoOptions>(configuration.GetSection(sectionName));

            return services;
        }
    }
}
