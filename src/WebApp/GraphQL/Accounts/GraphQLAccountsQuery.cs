using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using QuickShop.Domain.Accounts.Authentication;
using QuickShop.Domain.Ping;

namespace QuickShop.WebApp.GraphQL.Accounts
{
    public class GraphQLAccountsQuery : ObjectGraphType
    {
        private readonly IUserService _userService;

        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        public GraphQLAccountsQuery(IUserService userService)
        {
            _userService = userService;

            FieldAsync<UserType>(
                name: "user",
                arguments: new QueryArguments
                {
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "ID of the user" }
                },
                resolve: ResolveUserQueryAsync
            );
        }

        private async Task<object> ResolveUserQueryAsync(IResolveFieldContext<object> context)
        {
            string id = context.GetArgument<string>("id");
            return await _userService.FindUserByIdOrDefaultAsync(id);
        }

        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<UserType>();
            services.AddScoped<UserCredentialsType>();
            services.AddScoped<GraphQLAccountsQuery>();
        }
    }
}