using System;

namespace QuickShop.Domain.Abstractions
{
    public interface IAggregateRoot
    {
        string Id { get; }

        public DateTimeOffset CreatedOn { get; }

        public DateTimeOffset UpdatedOn { get; }
    }
}
