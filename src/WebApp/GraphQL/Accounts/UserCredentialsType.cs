using System.Diagnostics.CodeAnalysis;
using GraphQL.Types;
using QuickShop.Domain.Accounts.Model.UserAggregate;

namespace QuickShop.WebApp.GraphQL.Accounts
{
    public class UserCredentialsType : ObjectGraphType<UserCredentials>
    {
        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        public UserCredentialsType()
        {
            Name = nameof(UserCredentials);

            Field(_ => _.Username).Description("Username");
        }
    }
}