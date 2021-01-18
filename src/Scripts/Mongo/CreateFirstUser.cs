using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using QuickShop.Domain.Accounts.Authentication.HashingAlgorithm;
using QuickShop.Domain.Accounts.Model.UserAggregate;
using QuickShop.Extensions.Configuration;
using QuickShop.Repository.Mongo;
using QuickShop.Repository.Mongo.Accounts;

namespace QuickShop.Scripts.Mongo
{
    internal class CreateFirstUser : IScript
    {
        private readonly MongoClientWrapper _wrapper;

        public CreateFirstUser(MongoClientWrapper wrapper)
        {
            _wrapper = wrapper;
        }

        public async Task Run()
        {
            var userRepository = new MongoUserRepository(_wrapper);

            var found = await userRepository.FindByUsernameOrDefaultAsync("first");

            if (found != null)
                throw new InvalidOperationException("Already exists!");

            string passwordHash = new Sha512HashingAlgorithm().Hash("password");
            User user = new User(DateTimeOffset.Now, new UserCredentials("first", passwordHash));

            await userRepository.CreateAsync(user);
        }
    }
}
