using System;
using System.Diagnostics.CodeAnalysis;
using Domain;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace QuickShop.Domain.Accounts.Model.UserAggregate
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    [BsonIgnoreExtraElements]
    public class User : IAggregateRoot
    {
        public User(DateTimeOffset createdOn, UserCredentials credentials)
        {
            CreatedOn = createdOn;
            Credentials = credentials;
        }

        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Token { get; private set; }

        public DateTimeOffset CreatedOn { get; private set; }

        public UserCredentials Credentials { get; private set; }
    }
}
