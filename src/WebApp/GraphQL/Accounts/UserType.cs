using System.Diagnostics.CodeAnalysis;
using GraphQL.Types;
using QuickShop.Domain.Accounts.Model.UserAggregate;
using QuickShop.Domain.Ping.Model;

namespace QuickShop.WebApp.GraphQL.Accounts
{
    public class UserType : ObjectGraphType<User>
    {
        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        public UserType()
        {
            Name = nameof(User);

            Field(_ => _.Credentials, type: typeof(UserCredentialsType)).Description("User credentials");
        }
    }
}
