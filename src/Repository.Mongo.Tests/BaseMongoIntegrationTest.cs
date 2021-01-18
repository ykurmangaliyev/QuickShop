using System;
using System.Configuration;
using System.IO;
using MongoDB.Driver;
using QuickShop.Extensions.Configuration;

namespace QuickShop.Repository.Mongo.Tests
{
    public abstract class BaseMongoIntegrationTest
    {
        protected MongoClientWrapper ClientWrapper { get; }

        protected BaseMongoIntegrationTest()
        {
            var configuration = ConfigurationExtensions.LoadConfigurationUserFile("MongoIntegrationTests");

            string connectionString = configuration.ConnectionStrings.ConnectionStrings["Mongo"].ConnectionString;
            string databaseName = configuration.AppSettings.Settings["DatabaseName"].Value;

            var mongoClientSettings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

            // Uncomment this part to debug Mongo commands:
            
            /*
            mongoClientSettings.ClusterConfigurator = cb => {
                cb.Subscribe<CommandStartedEvent>(e => { Console.WriteLine($"{e.CommandName} - {e.Command.ToJson()}"); });
            };
            */

            var client = new MongoClient(mongoClientSettings);

            ClientWrapper = new MongoClientWrapper(client, databaseName);
        }
    }
}