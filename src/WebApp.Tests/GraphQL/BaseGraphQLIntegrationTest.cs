using System;
using System.Threading.Tasks;
using GraphQL.NewtonsoftJson;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuickShop.DependencyInjection;
using QuickShop.DependencyInjection.Repository;
using QuickShop.Repository.Abstractions;
using QuickShop.WebApp.GraphQL;

namespace QuickShop.WebApp.Tests.GraphQL
{
    public class BaseGraphQLIntegrationTest
    {
        private readonly IServiceProvider _serviceProvider;

        public BaseGraphQLIntegrationTest()
        {
            var services = new ServiceCollection();
            GraphQLSchema.RegisterAllServices(services);

            services.AddQuickShop();
            services.AddSingleton<IDatabaseContext>(_ => new InMemoryDatabaseContext());

            _serviceProvider = services.BuildServiceProvider();
        }

        protected async Task<string> RunGraphQLQuery(string query)
        {
            var schema = new GraphQLSchema(_serviceProvider);
            
            return await schema.ExecuteAsync(options =>
            {
                options.ThrowOnUnhandledException = true;
                options.Query = query;
            });
        }

        protected IDatabaseContext GetDatabaseContext()
        {
            return _serviceProvider.GetRequiredService<IDatabaseContext>();
        }

        protected JObject AssertHasData(string json, string key)
        {
            JObject parsed = JsonConvert.DeserializeObject<JObject>(json);

            Assert.IsNotNull(parsed["data"], "Data has not been found, response = " + json);
            Assert.IsInstanceOfType(parsed["data"], typeof(JObject));

            JObject data = (JObject)parsed["data"];

            Assert.IsNotNull(data[key]);
            Assert.IsInstanceOfType(data[key], typeof(JObject));

            return (JObject)data[key];
        }

        // TODO: fix code duplication
        protected void AssertHasNoData(string json, string key)
        {
            JObject parsed = JsonConvert.DeserializeObject<JObject>(json);

            Assert.IsNotNull(parsed["data"], "Data has not been found, response = " + json);
            Assert.IsInstanceOfType(parsed["data"], typeof(JObject));

            JObject data = (JObject)parsed["data"];

            Assert.IsNotNull(data[key]);

            Assert.AreEqual(JTokenType.Null, data[key].Type);
        }
    }
}