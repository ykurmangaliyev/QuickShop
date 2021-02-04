using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using QuickShop.Repository.Abstractions;

namespace QuickShop.Repository.Mongo.Tests.Base
{
    [TestClass]
    public class MongoDatabaseContextTests : AbstractMongoIntegrationTest
    {
        [TestCategory(nameof(IDatabaseContext.PingAsync))]
        [TestMethod]
        public async Task MongoClient_Should_HaveConnectionToRemoteMongoCluster()
        {
            var databaseContext = CreateDatabaseContext();
            var pingResult = await databaseContext.PingAsync();

            Assert.IsTrue(pingResult.HasValue);
            Assert.IsTrue(pingResult.Value > 0.0);
        }

        [TestCategory(nameof(IDatabaseContext.CreateAsync))]
        [TestMethod]
        public async Task DatabaseContext_When_CreateAsyncCalled_Should_CreateObject()
        {
            // Arrange
            var databaseContext = CreateDatabaseContext();
            var testObject = CreateTestObject();

            // Act - create object using database context
            await databaseContext.CreateAsync(testObject);

            // Assert - the object has been written to Mongo
            Assert.IsFalse(String.IsNullOrWhiteSpace(testObject.Id));

            var collection = databaseContext.GetCollection<TestAggregateRoot>();
            var foundObject = await collection.AsQueryable().Where(x => x.Id == testObject.Id).SingleAsync();

            Assert.AreEqual(true, foundObject.Boolean);
            Assert.AreEqual(42, foundObject.Integer);
            Assert.AreEqual("test", foundObject.String);

            // Clean up
            await collection.DeleteOneAsync(Builders<TestAggregateRoot>.Filter.Eq("id", foundObject.Id));
        }

        [TestCategory(nameof(IDatabaseContext.LoadAsync))]
        [TestMethod]
        public async Task DatabaseContext_When_LoadAsyncCalled_And_ObjectExists_Should_FindObject()
        {
            // Arrange
            var databaseContext = CreateDatabaseContext();
            var collection = databaseContext.GetCollection<TestAggregateRoot>();
            var testObject = CreateTestObject();

            await collection.InsertOneAsync(testObject);

            // Act - call LoadAsync
            var foundObject = await databaseContext.LoadAsync<TestAggregateRoot>(testObject.Id);

            // Assert - LoadAsync should have returned the object by ID
            Assert.IsNotNull(foundObject);

            Assert.AreEqual(true, foundObject.Boolean);
            Assert.AreEqual(42, foundObject.Integer);
            Assert.AreEqual("test", foundObject.String);

            // Clean up
            await collection.DeleteOneAsync(Builders<TestAggregateRoot>.Filter.Eq("id", foundObject.Id));
        }

        [TestCategory(nameof(IDatabaseContext.LoadAsync))]
        [TestMethod]
        public async Task DatabaseContext_When_LoadAsyncCalled_And_ObjectDoesNotExist_Should_ReturnNull()
        {
            // Arrange
            var databaseContext = CreateDatabaseContext();

            // Assert - LoadAsync should return null by non-existing ID
            var foundObject = await databaseContext.LoadAsync<TestAggregateRoot>("not-existing-id");

            Assert.IsNull(foundObject);
        }

        [TestCategory(nameof(IDatabaseContext.Query))]
        [TestMethod]
        public async Task DatabaseContext_Query_Should_TranslateAndExecuteLinqQueries()
        {
            // Arrange
            var databaseContext = CreateDatabaseContext();
            var collection = databaseContext.GetCollection<TestAggregateRoot>();

            var testObject = CreateTestObject();
            await collection.InsertOneAsync(testObject);

            // Act - call Query
            var foundObjects = databaseContext.Query<TestAggregateRoot>().Where(t => t.Id == testObject.Id).ToArray();

            // Assert - Query should return array of objects of size 1
            Assert.AreEqual(1, foundObjects.Length);

            Assert.AreEqual(true, foundObjects[0].Boolean);
            Assert.AreEqual(42, foundObjects[0].Integer);
            Assert.AreEqual("test", foundObjects[0].String);

            // Clean up
            await collection.DeleteOneAsync(Builders<TestAggregateRoot>.Filter.Eq("id", foundObjects[0].Id));
        }

        [TestCategory(nameof(IDatabaseContext.DeleteAsync))]
        [TestMethod]
        public async Task DatabaseContext_DeleteAsync_When_ObjectExists_Should_RemoveObjects()
        {
            // Arrange
            var databaseContext = CreateDatabaseContext();
            var collection = databaseContext.GetCollection<TestAggregateRoot>();

            var testObject = CreateTestObject();
            await collection.InsertOneAsync(testObject);

            string testObjectId = testObject.Id;
            
            var foundBeforeDelete = await collection.AsQueryable().SingleOrDefaultAsync(t => t.Id == testObjectId);
            Assert.IsNotNull(foundBeforeDelete);

            // Act - call DeleteAsync
            await databaseContext.DeleteAsync<TestAggregateRoot>(testObject.Id);

            // Assert - the object should has been removed
            var foundAfterDelete = await collection.AsQueryable().SingleOrDefaultAsync(t => t.Id == testObjectId);
            Assert.IsNull(foundAfterDelete);
        }
    }
}
