using System;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuickShop.Domain.Accounts.Model.UserAggregate;
using QuickShop.Repository.Mongo.CollectionNameMapping;
using QuickShop.Repository.Mongo.Configuration;

namespace QuickShop.Repository.Mongo.Tests
{
    public abstract class BaseMongoIntegrationTest
    {
        private readonly MongoOptions _mongoOptions;

        protected MongoDatabaseContext CreateDatabaseContext(Func<Type, string> collectionNameMapper)
            => CreateDatabaseContext(new CodedCollectionNameMapper(collectionNameMapper));

        protected MongoDatabaseContext CreateDatabaseContext(ICollectionNameMapper collectionNameMapper)
        {
            return new(new OptionsWrapper<MongoOptions>(_mongoOptions), collectionNameMapper);
        }

        protected BaseMongoIntegrationTest()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("test-settings.json")
                .AddJsonFile("test-settings.user.json", optional: true)
                .Build();

            _mongoOptions = configuration.GetSection(MongoOptions.Mongo).Get<MongoOptions>();
        }
    }
}