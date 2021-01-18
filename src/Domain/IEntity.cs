using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Domain
{
    public interface IEntity
    {
        /// <summary>
        /// For the sake of consistency, all entities in the project should use string "token" identifiers.
        /// In order to generate this value, mark your with the following attribute definition:
        /// [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        /// </summary>
        string Token { get; }
    }
}