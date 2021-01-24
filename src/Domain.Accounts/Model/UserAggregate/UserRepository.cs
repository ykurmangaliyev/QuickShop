using System.Linq;
using System.Threading.Tasks;
using QuickShop.Repository.Abstractions;

namespace QuickShop.Domain.Accounts.Model.UserAggregate
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public UserRepository(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<User> CreateAsync(string username, string passwordHash)
        {
            User user = new User(new UserCredentials(username, passwordHash));
            return await _databaseContext.CreateAsync(user);
        }

        public Task<User> FindByIdOrDefaultAsync(string id)
        {
            return _databaseContext.LoadAsync<User>(id);
        }

        public Task<User> FindByUsernameOrDefaultAsync(string username)
        {
            var user = _databaseContext.Query<User>().SingleOrDefault(u => u.Credentials.Username == username);
            return Task.FromResult(user);
        }

        public async Task DeleteAsync(string id)
        {
            await _databaseContext.DeleteAsync<User>(id);
        }
    }
}
