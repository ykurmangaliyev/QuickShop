using System;
using Logging;
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

namespace QuickShop.DependencyInjection.Logger
{
    public static class LoggerProviderExtensions
    {
        public static IQuickShopServiceBuilder AddConsoleLogger(this IQuickShopServiceBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.Services.AddSingleton<ILogger, ConsoleLogger>();

            return builder;
        }
    }
}
