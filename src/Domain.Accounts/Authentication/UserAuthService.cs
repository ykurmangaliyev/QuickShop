using System.Threading.Tasks;
using Logging;
using QuickShop.Domain.Accounts.Authentication.HashingAlgorithm;
using QuickShop.Domain.Accounts.Model.UserAggregate;
using QuickShop.Extensions.Security;

namespace QuickShop.Domain.Accounts.Authentication
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IHashingAlgorithm _hashingAlgorithm;

        private readonly IUserRepository _userRepository;

        private readonly ILogger _logger;

        public UserAuthService(IHashingAlgorithm hashingAlgorithm, IUserRepository userRepository, ILogger logger)
        {
            _hashingAlgorithm = hashingAlgorithm;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(string username, string password)
        {
            User user = await _userRepository.FindByUsernameOrDefaultAsync(username);

            if (user == null)
            {
                _logger.Info($"Authentication failed, invalid username, username={username}");
                return AuthenticationResult.Fail(AuthenticationResultCode.InvalidCredentials);
            }

            string expectedPasswordHash = _hashingAlgorithm.Hash(password);
            string actualPasswordHash = user.Credentials.PasswordHash;

            bool hashesAreEqual = actualPasswordHash.TimeConstantEquals(expectedPasswordHash);

            if (!hashesAreEqual)
            {
                _logger.Info($"Authentication failed, invalid password, username={username}");
                return AuthenticationResult.Fail(AuthenticationResultCode.InvalidCredentials);
            }

            _logger.Info($"Authentication succeeded, username={username}");
            return AuthenticationResult.Success(user);
        }
    }
}