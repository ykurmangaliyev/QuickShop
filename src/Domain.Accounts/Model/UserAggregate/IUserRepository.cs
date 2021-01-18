using System.Threading.Tasks;

namespace QuickShop.Domain.Accounts.Model.UserAggregate
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(string username, string passwordHash);

        Task<User> FindByUsernameOrDefaultAsync(string username);
    }
}