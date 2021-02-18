using System.Threading.Tasks;
using QuickShop.Domain.Accounts.Model.UserAggregate;

namespace QuickShop.Domain.Accounts.Authentication
{
    public interface IUserService
    {
        Task<User> FindUserByIdOrDefaultAsync(string id);
    }
}