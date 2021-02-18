using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace QuickShop.Domain.Abstractions
{
    /// <summary>
    /// This class represents a base AggregateRoot
    /// </summary>
    /// <remarks>
    /// Implementation of this class has multiple leaked abstractions:
    /// - [BsonIgnoreExtraElements] attribute
    /// - [BsonId] attribute
    /// This leakage can be resolved by implementing separate DAO models and mapping domain models to DAO models and back.
    /// However, it comes with huge cost when it comes to development speed and complexity of the code, while bringing no improvement whatsoever.
    /// This can be reconsidered in future.
    /// </remarks>
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class AbstractAggregateRoot : IAggregateRoot
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; protected set; }

        public DateTimeOffset CreatedOn { get; private set; } = DateTimeOffset.Now;

        public DateTimeOffset UpdatedOn { get; protected set; } = DateTimeOffset.Now;
    }
}