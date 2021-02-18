using System;
using System.Collections;
using System.Collections.Generic;
using GraphQL;
using GraphQL.Execution;
using GraphQL.NewtonsoftJson;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using QuickShop.WebApp.GraphQL.Accounts;
using QuickShop.WebApp.GraphQL.Ping;

namespace QuickShop.WebApp.GraphQL
{
    public class GraphQLSchema : Schema
    {
        // TODO: fix service locator
        public GraphQLSchema(IServiceProvider services) : base(services)
        {
            Query = services.GetRequiredService<GraphQLQueryRoot>();
        }

        public static void RegisterAllServices(IServiceCollection services)
        {
            services.AddScoped<IDocumentExecuter, DocumentExecuter>();
            services.AddScoped<DocumentWriter>(_ => new DocumentWriter(
                new ErrorInfoProvider(options =>
                {
                    options.ExposeExtensions = true;
                    options.ExposeExceptionStackTrace = true;
                }))
            );

            GraphQLPingQuery.RegisterServices(services);
            GraphQLAccountsQuery.RegisterServices(services);

            services.AddScoped<GraphQLQueryRoot>(provider => new GraphQLQueryRoot(provider, new []
            {
                typeof(GraphQLPingQuery),
                typeof(GraphQLAccountsQuery)
            }));

            services.AddScoped<ISchema, GraphQLSchema>();
        }
    }
}