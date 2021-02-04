using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace QuickShop.Repository.Mongo.Tests.Base
{
    [TestClass]
    public class MongoSessionTransactionTests : AbstractMongoIntegrationTest
    {
        [TestMethod]
        public async Task When_TransactionIsCommitted_Should_WriteFromMongo()
        {
            // Arrange
            var databaseContext = CreateDatabaseContext();
            var collection = databaseContext.GetCollection<TestAggregateRoot>();
            var testObject = CreateTestObject();

            // Act - create transaction, write and commit
            using (var transaction = await databaseContext.StartTransactionAsync())
            {
                await databaseContext.CreateAsync(testObject);
                await transaction.CommitAsync();
            }
            
            // Assert - the model should has been created
            var loadedObject = await collection.AsQueryable().SingleOrDefaultAsync(t => t.Id == testObject.Id);
            Assert.IsNotNull(loadedObject);
        }

        [TestMethod]
        public async Task When_TransactionIsAborted_Should_NotWriteToMongo()
        {
            // Arrange
            var databaseContext = CreateDatabaseContext();
            var collection = databaseContext.GetCollection<TestAggregateRoot>();
            var testObject = CreateTestObject();

            // Act - create transaction, write and commit
            using (var transaction = await databaseContext.StartTransactionAsync())
            {
                await databaseContext.CreateAsync(testObject);
                await transaction.RollbackAsync();
            }

            // Assert - the model shouldn't has been created
            var loadedObject = await collection.AsQueryable().SingleOrDefaultAsync(t => t.Id == testObject.Id);
            Assert.IsNull(loadedObject);
        }

        [TestMethod]
        public async Task When_TransactionIsNeitherCommittedNorRolledBack_Should_ImplicitlyRollback()
        {
            // Arrange
            var databaseContext = CreateDatabaseContext();
            var collection = databaseContext.GetCollection<TestAggregateRoot>();
            var testObject = CreateTestObject();

            // Act - create transaction, write and commit
            using (await databaseContext.StartTransactionAsync())
            {
                await databaseContext.CreateAsync(testObject);
            }

            // Assert - the model shouldn't has been created
            var loadedObject = await collection.AsQueryable().SingleOrDefaultAsync(t => t.Id == testObject.Id);
            Assert.IsNull(loadedObject);
        }

        [TestMethod]
        public async Task When_UsingSameDatabaseContextForConsequentTransactions_Should_GracefullyDisposeTransactions()
        {
            // Arrange
            var databaseContext = CreateDatabaseContext();
            var collection = databaseContext.GetCollection<TestAggregateRoot>();
            
            var testObjects = Enumerable.Range(0, 5).Select(_ => CreateTestObject()).ToArray();

            // Act
            // #0: explicit commit
            using (var transaction = await databaseContext.StartTransactionAsync())
            {
                await databaseContext.CreateAsync(testObjects[0]);
                await transaction.CommitAsync();
            }

            // #1: write outside of transaction
            await databaseContext.CreateAsync(testObjects[1]);

            // #2: explicit rollback
            using (var transaction = await databaseContext.StartTransactionAsync())
            {
                await databaseContext.CreateAsync(testObjects[2]);
                await transaction.RollbackAsync();
            }

            // #3: implicit rollback on dispose
            using (await databaseContext.StartTransactionAsync())
            {
                await databaseContext.CreateAsync(testObjects[3]);
            }

            // #4: write outside of transaction
            await databaseContext.CreateAsync(testObjects[4]);

            // Assert - #0, #1, #4 should have been persisted
            string[] allIds = testObjects.Select(t => t.Id).ToArray();
            string[] expectedIds = { allIds[0], allIds[1], allIds[4] };

            var foundList = await collection.AsQueryable().Where(t => allIds.Contains(t.Id)).ToListAsync();
            
            CollectionAssert.AreEquivalent(expectedIds, foundList.Select(t => t.Id).ToArray());
        }

        [TestMethod]
        public async Task When_TryingToStartATransaction_And_AlreadyInTransaction_Should_ThrowInvalidOperationException()
        {
            // Arrange
            var databaseContext = CreateDatabaseContext();

            using var transaction1 = await databaseContext.StartTransactionAsync();

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await databaseContext.StartTransactionAsync();
            });
        }
    }
}