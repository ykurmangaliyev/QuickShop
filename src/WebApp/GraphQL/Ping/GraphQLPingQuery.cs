using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using QuickShop.Domain.Ping;

namespace QuickShop.WebApp.GraphQL.Ping
{
    public class GraphQLPingQuery : ObjectGraphType
    {
        private readonly IPingService _pingService;

        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        public GraphQLPingQuery(IPingService pingService)
        {
            _pingService = pingService;

            FieldAsync<PingCheckType>(
                name: "ping",
                arguments: new QueryArguments(),
                resolve: ResolvePingQueryAsync
            );
        }

        private async Task<object> ResolvePingQueryAsync(IResolveFieldContext<object> context)
        {
            return await _pingService.Ping();
        }

        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<PingCheckType>();
            services.AddScoped<GraphQLPingQuery>();
        }
    }
}