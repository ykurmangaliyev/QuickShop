using System;
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
        }
        
        public UserCredentials Credentials { get; private set; }
    }
}
