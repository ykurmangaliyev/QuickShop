using System.Threading.Tasks;

namespace QuickShop.Domain.Accounts.Authentication
{
    public interface IUserAuthService
    {
        Task<AuthenticationResult> AuthenticateAsync(string username, string password);
    }
}
