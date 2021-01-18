using MongoDB.Bson.Serialization.Attributes;

namespace QuickShop.Domain.Accounts.Model.UserAggregate
{
    public class UserCredentials
    {
        public UserCredentials(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
        }

        public string Username { get; }

        public string PasswordHash { get; }
    }
}