using System;
using System.Collections;
using System.Collections.Generic;
using GraphQL;
using GraphQL.NewtonsoftJson;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using QuickShop.WebApp.GraphQL.Ping;

namespace QuickShop.WebApp.GraphQL
{
    public class GraphQLSchema : Schema
    {
        public GraphQLSchema(IServiceProvider services) : base(services)
        {
            Query = services.GetRequiredService<GraphQLQueryRoot>();
        }

        public static void RegisterAllServices(IServiceCollection services)
        {
            services.AddScoped<IDocumentExecuter, DocumentExecuter>();
            services.AddScoped<IDocumentWriter, DocumentWriter>();

            GraphQLPingQuery.RegisterServices(services);

            services.AddScoped<GraphQLQueryRoot>(provider => new GraphQLQueryRoot(provider, new []
            {
                typeof(GraphQLPingQuery),
            }));

            services.AddScoped<ISchema, GraphQLSchema>();
        }
    }
}