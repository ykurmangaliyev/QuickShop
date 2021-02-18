using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using QuickShop.Domain.Accounts.Model.UserAggregate;
using QuickShop.Repository.Abstractions;
using QuickShop.Repository.Mongo;
using QuickShop.Repository.Mongo.CollectionNameMapping;
using QuickShop.Repository.Mongo.Configuration;

namespace QuickShop.DependencyInjection.Repository
{
    public static class RepositoryInMemoryExtensions
    {
        public static IQuickShopServiceBuilder AddInMemoryRepository(this IQuickShopServiceBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.Services.AddSingleton<IDatabaseContext, InMemoryDatabaseContext>();

            return builder;
        }
    }
}
