using System;
using System.Threading.Tasks;
using Logging;
using MongoDB.Driver;
using QuickShop.Domain.Accounts.Authentication.HashingAlgorithm;
using QuickShop.Domain.Accounts.Model.UserAggregate;
using QuickShop.Repository.Abstractions;
using QuickShop.Repository.Mongo;

namespace QuickShop.Scripts.Mongo
{
    internal class CreateFirstUser : IScript
    {
        private const string Username = "first";
        private const string Password = "password";

        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;
        private readonly IHashingAlgorithm _hashingAlgorithm;

        public CreateFirstUser(IUserRepository userRepository, ILogger logger, IHashingAlgorithm hashingAlgorithm)
        {
            _userRepository = userRepository;
            _logger = logger;
            _hashingAlgorithm = hashingAlgorithm;
        }

        public async Task Run()
        {
            var found = await _userRepository.FindByUsernameOrDefaultAsync("first");

            if (found != null)
            {
                _logger.Warning("The first user already exists!");
                return;
            }

            var created = await _userRepository.CreateAsync(Username, _hashingAlgorithm.Hash(Password));
            _logger.Info($"Created a first user! username=#{created.Credentials.Username}, id=#{created.Id}, password=#{created.Credentials.PasswordHash}");
        }
    }
}
