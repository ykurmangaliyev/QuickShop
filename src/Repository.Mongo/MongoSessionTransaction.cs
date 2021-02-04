using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using QuickShop.Repository.Abstractions;

namespace QuickShop.Repository.Mongo
{
    /// <inheritdoc cref="IDatabaseTransaction"/>
    /// <summary>
    /// Implementation of <see cref="IDatabaseTransaction"/> using Mongo <see cref="IClientSessionHandle"/>
    /// </summary>
    internal class MongoSessionTransaction : IDatabaseTransaction
    {
        public bool IsDisposed { get; private set; } = false;

        public IClientSessionHandle SessionHandle { get; }

        public MongoSessionTransaction(IClientSessionHandle clientSessionHandle)
        {
            SessionHandle = clientSessionHandle ?? throw new ArgumentNullException(nameof(clientSessionHandle));
            SessionHandle.StartTransaction();
        }

        public Task CommitAsync()
        {
            ThrowIfDisposed();
            return SessionHandle.CommitTransactionAsync();
        }

        public Task RollbackAsync()
        {
            ThrowIfDisposed();
            return SessionHandle.AbortTransactionAsync();
        }

        /// <summary>
        /// Commits and synchronously waits until completion.
        /// Use CommitAsync to commit in async scenarios for better performance and to avoid deadlocks.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;

            SessionHandle.Dispose();

            IsDisposed = true;
        }

        /// <summary>Throws if disposed</summary>
        /// <exception cref="T:System.ObjectDisposedException"></exception>
        protected void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);
        }
    }
}