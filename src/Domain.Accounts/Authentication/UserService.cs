using System;
using System.Threading.Tasks;
using QuickShop.Domain.Accounts.Model.UserAggregate;

namespace QuickShop.Domain.Accounts.Authentication
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> FindUserByIdOrDefaultAsync(string id)
        {
            return await _userRepository.FindByIdOrDefaultAsync(id);
        }
    }
}