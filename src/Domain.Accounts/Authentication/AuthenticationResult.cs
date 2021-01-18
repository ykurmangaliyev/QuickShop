using QuickShop.Domain.Accounts.Model.UserAggregate;

namespace QuickShop.Domain.Accounts.Authentication
{
    public readonly struct AuthenticationResult
    {
        private AuthenticationResult(AuthenticationResultCode code, User user)
        {
            Code = code;
            User = user;
        }

        public AuthenticationResultCode Code { get; }

        public User User { get; }

        public bool IsSuccess => Code == AuthenticationResultCode.Success;

        public static AuthenticationResult Success(User user)
        {
            return new AuthenticationResult(AuthenticationResultCode.Success, user);
        }

        public static AuthenticationResult Fail(AuthenticationResultCode code)
        {
            return new AuthenticationResult(code, null);
        }
    }
}