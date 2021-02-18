using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuickShop.Domain.Accounts.Authentication;
using QuickShop.Domain.Ping;
using QuickShop.Repository.Abstractions;

namespace QuickShop.WebApp.Tests.GraphQL
{
    [TestClass]
    public class GraphQLPingQueryTests : BaseGraphQLIntegrationTest
    {
        [TestMethod]
        public async Task PingQuery_Should_Succeed()
        {
            string query = @"query Ping {
  ping {  
    serverTime
    databaseStatus
    databasePing
  }
}";
            string json = await RunGraphQLQuery(query);

            JObject data = AssertHasData(json, "ping");

            Assert.AreEqual(0.0, data["databasePing"]);
            Assert.AreEqual(true, data["databaseStatus"]);
            Assert.IsNotNull(data["serverTime"]);
        }
    }
}
