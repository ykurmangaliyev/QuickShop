using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace QuickShop.Repository.Mongo.Tests
{
    [TestClass]
    public class MongoConnectionTests : BaseMongoIntegrationTest
    {
        private const string DefaultCollectionName = "default";

        [TestMethod]
        public async Task Should_ConnectToRemoteMongoCluster()
        {
            var collection = CreateDatabaseContext(_ => DefaultCollectionName).GetCollection<BsonDocument>();

            string objectId = ObjectId.GenerateNewId().ToString();

            // Write
            var document = new BsonDocument
            {
                {"id", objectId },
                {"key", "value" },
            };

            await collection.InsertOneAsync(document);

            // Read
            var readDocument = await collection.AsQueryable().Where(x => x["id"] == objectId).SingleAsync();

            Assert.AreEqual("value", readDocument["key"]);

            // Clean up
            await collection.DeleteOneAsync(Builders<BsonDocument>.Filter.Eq("id", objectId));
        }
    }
}
