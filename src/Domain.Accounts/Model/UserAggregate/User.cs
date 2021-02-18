using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using QuickShop.Domain.Abstractions;

namespace QuickShop.Domain.Accounts.Model.UserAggregate
{
    public class User : AbstractAggregateRoot
    {
        public User(UserCredentials credentials)
        {
            Credentials = credentials;
            StripeDetails = new StripeDetails();
            EmailDetails = new EmailDetails();
        }

        public User(string id, UserCredentials credentials) : this(credentials)
        {
            Id = id;
        }
        
        public UserCredentials Credentials { get; private set; }

        public StripeDetails StripeDetails { get; private set; }

        public EmailDetails EmailDetails { get; private set; }
    }
}
