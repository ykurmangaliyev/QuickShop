using System;
using System.Threading.Tasks;

namespace QuickShop.Repository.Abstractions
{
    /// <summary>
    /// This interface is designed to abstract database transactions.
    /// Transactions should be implicitly aborted when the instance is disposed, unless it is explicitly committed.
    /// </summary>
    /// <remarks>
    /// For the current implementation of Mongo, transaction = session
    /// </remarks>
    public interface IDatabaseTransaction : IDisposable
    {
        Task CommitAsync();

        Task RollbackAsync();
    }
}