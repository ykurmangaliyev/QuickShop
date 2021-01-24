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
    public static class RepositoryMongoExtensions
    {
        public static IQuickShopServiceBuilder AddMongoRepository(this IQuickShopServiceBuilder builder)
            => AddMongoRepository(builder, _ => { });

        public static IQuickShopServiceBuilder AddMongoRepository(this IQuickShopServiceBuilder builder, Action<MongoOptions> options)
        {
            if (builder == null) 
                throw new ArgumentNullException(nameof(builder));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            builder.Services.Configure(options);

            builder.Services.AddSingleton<ICollectionNameMapper, TypeCollectionNameMapper>();
            builder.Services.AddSingleton<MongoClient>();
            builder.Services.AddSingleton<IDatabaseContext, MongoDatabaseContext>();

            return builder;
        }

        public static IServiceCollection ConfigureMongoRepositoryOptions(this IServiceCollection services, IConfiguration configuration) 
            => ConfigureMongoRepositoryOptions(services, configuration, MongoOptions.Mongo);

        public static IServiceCollection ConfigureMongoRepositoryOptions(this IServiceCollection services, IConfiguration configuration, string sectionName)
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
