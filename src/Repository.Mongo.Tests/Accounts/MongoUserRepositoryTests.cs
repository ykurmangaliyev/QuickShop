using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using QuickShop.Domain.Accounts.Model.UserAggregate;
using QuickShop.Repository.Mongo.Accounts;

namespace QuickShop.Repository.Mongo.Tests.Accounts
{
    [TestClass]
    public class MongoUserRepositoryTests : BaseMongoIntegrationTest
    {
        [TestMethod]
        public async Task Should_WriteUserModel_And_ReadItBack()
        {
            var repository = new MongoUserRepository(ClientWrapper);

            var user = new User(DateTimeOffset.Now, new UserCredentials("username", "password"));

            // Write
            var insertedUser = await repository.CreateAsync(user);
            Assert.IsFalse(String.IsNullOrWhiteSpace(insertedUser.Token));
            Assert.AreEqual(user.CreatedOn, user.CreatedOn);
            Assert.AreEqual(user.Credentials.Username, insertedUser.Credentials.Username);
            Assert.AreEqual(user.Credentials.PasswordHash, insertedUser.Credentials.PasswordHash);

            // Read by token succeeds
            var readUser = await repository.FindByTokenOrDefaultAsync(insertedUser.Token);
            Assert.AreEqual(insertedUser.Token, readUser.Token);
            Assert.AreEqual(user.CreatedOn, readUser.CreatedOn);
            Assert.AreEqual(user.Credentials.Username, readUser.Credentials.Username);
            Assert.AreEqual(user.Credentials.PasswordHash, readUser.Credentials.PasswordHash);

            // Read by username succeeds
            var readUserByUsername = await repository.FindByTokenOrDefaultAsync(insertedUser.Token);
            Assert.AreEqual(insertedUser.Token, readUserByUsername.Token);
            Assert.AreEqual(user.CreatedOn, readUserByUsername.CreatedOn);
            Assert.AreEqual(user.Credentials.Username, readUserByUsername.Credentials.Username);
            Assert.AreEqual(user.Credentials.PasswordHash, readUserByUsername.Credentials.PasswordHash);

            // Read by wrong token fails
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await repository.FindByTokenOrDefaultAsync("other-token");
            });
            
            // Remove
            await repository.DeleteAsync(insertedUser.Token);
        }
    }
}
