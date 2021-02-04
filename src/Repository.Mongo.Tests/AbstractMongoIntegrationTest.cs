using System;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using QuickShop.Domain.Accounts.Model.UserAggregate;
using QuickShop.Repository.Mongo.CollectionNameMapping;
using QuickShop.Repository.Mongo.Configuration;

namespace QuickShop.Repository.Mongo.Tests
{
    [TestClass]
    public abstract class AbstractMongoIntegrationTest
    {
        private static MongoOptions _mongoOptions;

        private static string _mongoCollectionPrefix;
        private static TestSuffixedCollectionNameMapperWrapper _collectionNameMapper;

        protected static MongoDatabaseContext CreateDatabaseContext()
        {
            return new(new OptionsWrapper<MongoOptions>(_mongoOptions), _collectionNameMapper);
        }

        protected static TestAggregateRoot CreateTestObject()
        {
            return new()
            {
                Boolean = true,
                Integer = 42,
                String = "test",
            };
        }

        [AssemblyInitialize]
        public static void InitializeDatabase(TestContext context)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("test-settings.json")
                .AddJsonFile("test-settings.user.json", optional: true)
                .Build();

            _mongoOptions = configuration.GetSection(MongoOptions.Mongo).Get<MongoOptions>();

            _mongoCollectionPrefix = $"test-run-{DateTime.Now:yyyy-MM-dd-HH-mm-ss-fff}-";
            context.WriteLine($"Configured Mongo to write to collection with prefix {_mongoCollectionPrefix}");

            _collectionNameMapper = new TestSuffixedCollectionNameMapperWrapper(
                new TypeCollectionNameMapper(),
                _mongoCollectionPrefix
            );

            CreateDatabaseContext().GetDatabase(); // create database
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            var database = CreateDatabaseContext().GetDatabase();

            foreach (string collectionName in database.ListCollectionNames().ToList())
            {
                if (collectionName.StartsWith(_mongoCollectionPrefix))
                {
                    database.DropCollection(collectionName);
                }
            }
        }
    }
}