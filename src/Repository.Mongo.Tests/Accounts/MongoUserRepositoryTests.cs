using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using QuickShop.Domain.Accounts.Model.UserAggregate;
using QuickShop.Repository.Mongo.CollectionNameMapping;

namespace QuickShop.Repository.Mongo.Tests.Accounts
{
    // TODO: it should not be tested here, Mongo should rely on user-specific logic. Instead, create a model for the test and check that the context works
    [TestClass]
    public class MongoUserRepositoryTests : AbstractMongoIntegrationTest
    {
        [TestMethod]
        public async Task Should_WriteUserModel_And_ReadItBack()
        {
            var repository = new UserRepository(CreateDatabaseContext());

            // Write
            var insertedUser = await repository.CreateAsync("username", "password-hash");
            Assert.IsFalse(String.IsNullOrWhiteSpace(insertedUser.Id));
            Assert.AreEqual("username", insertedUser.Credentials.Username);
            Assert.AreEqual("password-hash", insertedUser.Credentials.PasswordHash);

            // Read by token succeeds
            var readUser = await repository.FindByIdOrDefaultAsync(insertedUser.Id);
            Assert.AreEqual(insertedUser.Id, readUser.Id);
            Assert.AreEqual(insertedUser.CreatedOn, readUser.CreatedOn);
            Assert.AreEqual(insertedUser.Credentials.Username, readUser.Credentials.Username);
            Assert.AreEqual(insertedUser.Credentials.PasswordHash, readUser.Credentials.PasswordHash);

            // Read by username succeeds
            var readUserByUsername = await repository.FindByIdOrDefaultAsync(insertedUser.Id);
            Assert.AreEqual(insertedUser.Id, readUserByUsername.Id);
            Assert.AreEqual(insertedUser.CreatedOn, readUserByUsername.CreatedOn);
            Assert.AreEqual(insertedUser.Credentials.Username, readUserByUsername.Credentials.Username);
            Assert.AreEqual(insertedUser.Credentials.PasswordHash, readUserByUsername.Credentials.PasswordHash);

            // Read by wrong token fails
            var otherUser = await repository.FindByIdOrDefaultAsync("other-token");
            Assert.IsNull(otherUser);
            
            // Remove
            await repository.DeleteAsync(insertedUser.Id);
        }
    }
}
