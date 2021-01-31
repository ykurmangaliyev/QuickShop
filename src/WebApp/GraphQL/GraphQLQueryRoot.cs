using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using QuickShop.Domain.Ping;
using QuickShop.WebApp.GraphQL.Ping;

namespace QuickShop.WebApp.GraphQL
{
    public class GraphQLQueryRoot : ObjectGraphType
    {
        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        public GraphQLQueryRoot(IServiceProvider services, Type[] registeredQueryTypes)
        {
            foreach (var queryType in registeredQueryTypes)
            {
                var query = (ObjectGraphType)services.GetRequiredService(queryType);

                foreach (var field in query.Fields)
                {
                    Field(field.Type, field.Name, field.Description, field.Arguments, field.Resolver.Resolve);
                }
            }
        }

        private void Import<T>(IServiceProvider services) where T : ObjectGraphType
        {
            
        }
    }
}