using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using QuickShop.Domain.Accounts.Model.UserAggregate;

namespace QuickShop.Repository.Mongo.Accounts
{
    public class MongoUserRepository : MongoBaseRepository<User>, IUserRepository
    {
        private const string CollectionName = "users";

        public MongoUserRepository(MongoClientWrapper clientWrapper) : base(clientWrapper, CollectionName)
        {
        }

        public async Task<User> CreateAsync(string username, string passwordHash)
        {
            User user = new User(DateTimeOffset.Now, new UserCredentials(username, passwordHash));
            return await CreateAsync(user);
        }

        public async Task<User> FindByUsernameOrDefaultAsync(string username)
        {
            return await Query().Where(u => u.Credentials.Username == username).SingleOrDefaultAsync();
        }
    }
}
