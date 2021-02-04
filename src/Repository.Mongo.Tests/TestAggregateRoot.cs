using QuickShop.Domain.Abstractions;

namespace QuickShop.Repository.Mongo.Tests
{
    public class TestAggregateRoot : AbstractAggregateRoot
    {
        public bool Boolean { get; init; }

        public string String { get; init; }

        public int Integer { get; init; }
    }
}