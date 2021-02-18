using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QuickShop.Domain.Accounts.Model.UserAggregate;

namespace QuickShop.WebApp.Tests.GraphQL
{
    [TestClass]
    public class AccountsQueryTests : BaseGraphQLIntegrationTest
    {
        [TestMethod]
        public async Task When_UserExists_Should_ReturnUser()
        {
            // Arrange
            var user = new User("my_id", new UserCredentials("my_username", "my_hash"));

            await GetDatabaseContext().CreateAsync(user);

            // Act
            string query = @"{
    user(id: ""my_id"") {
        credentials {
            username
        }
    }
}";

            string json = await RunGraphQLQuery(query);

            // Assert
            JObject data = AssertHasData(json, "user");

            Assert.AreEqual("my_username", data.SelectToken("$.credentials.username"));
        }

        [TestMethod]
        public async Task When_UserNotFound_Should_ReturnNoData()
        {
            // Act
            string query = @"{
    user(id: ""another_id"") {
        credentials {
            username
        }
    }
}";
            string json = await RunGraphQLQuery(query);

            // Assert
            AssertHasNoData(json, "user");
        }
    }
}