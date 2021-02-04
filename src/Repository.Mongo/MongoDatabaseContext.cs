using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using QuickShop.Domain.Abstractions;
using QuickShop.Repository.Abstractions;
using QuickShop.Repository.Mongo.CollectionNameMapping;
using QuickShop.Repository.Mongo.Configuration;

namespace QuickShop.Repository.Mongo
{
    /// <summary>
    /// Mongo implementation of <see cref="IDatabaseContext"/>, which is primarily based on <see cref="MongoClient"/> NuGet package.
    /// </summary>
    /// <remarks>
    /// In this class, there are many places where we have "if active session exists, then call with session, otherwise call without session" logic.
    /// Unfortunately, there is no nicer way to do this, because Mongo client raises if you provide null session.
    /// Though, it is surprising that Mongo client package does not handle null session gracefully.
    /// </remarks>
    public class MongoDatabaseContext : IDatabaseContext
    {
        private readonly MongoOptions _mongoOptions;

        private readonly ICollectionNameMapper _collectionNameMapper;
        private readonly MongoClient _client;

        private MongoSessionTransaction _currentTransaction;

        public MongoDatabaseContext(IOptions<MongoOptions> mongoOptions, ICollectionNameMapper collectionNameMapper)
        {
            _mongoOptions = mongoOptions.Value;
            _collectionNameMapper = collectionNameMapper;
            _client = new MongoClient(_mongoOptions.ConnectionString);
        }

        /// <remarks>Internal for testing purposes</remarks>
        internal IMongoDatabase GetDatabase()
        {
            return _client.GetDatabase(_mongoOptions.DatabaseName);
        }

        /// <remarks>Internal for testing purposes</remarks>
        internal IMongoCollection<T> GetCollection<T>()
        {
            string collectionName = _collectionNameMapper.GetCollectionName<T>();
            return GetDatabase().GetCollection<T>(collectionName);
        }

        public Task<double?> PingAsync()
        {
            var database = _client.GetDatabase(_mongoOptions.DatabaseName);
            var activeSessionHandle = GetActiveSessionHandle();

            DateTime start = DateTime.Now;

            bool pingReceived = activeSessionHandle != null
                ? database.RunCommandAsync(activeSessionHandle, (Command<BsonDocument>)"{ping:1}").Wait(_client.Settings.ConnectTimeout)
                : database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(_client.Settings.ConnectTimeout);
                
            double? elapsed = pingReceived ? (DateTime.Now - start).TotalMilliseconds : null;

            return Task.FromResult(elapsed);
        }

        public async Task<T> CreateAsync<T>(T model) where T : IAggregateRoot
        {
            var activeSessionHandle = GetActiveSessionHandle();

            if (activeSessionHandle != null)
            {
                await GetCollection<T>().InsertOneAsync(activeSessionHandle, model);
            }
            else
            {
                await GetCollection<T>().InsertOneAsync(model);
            }
            
            return model;
        }

        /// <inheritdoc cref="IDatabaseContext"/>
        public async Task<T> LoadAsync<T>(string id) where T : IAggregateRoot
        {
            return await Query<T>().SingleOrDefaultAsync(m => m.Id == id);
        }

        /// <remarks>
        /// This is an explicit interface implementation of <see cref="IDatabaseContext.Query{T}"/>.
        /// </remarks>
        /// <typeparam name="T">Aggregate root type</typeparam>
        /// <returns>Generic interface for building queries</returns>
        IQueryable<T> IDatabaseContext.Query<T>()
        {
            return Query<T>();
        }

        /// <remarks>
        /// This class implements <see cref="Query{T}"/> to return <see cref="IMongoQueryable{T}"/>, which provides a wider range of features than <see cref="IQueryable{T}"/>.
        /// Methods of this class can use this method to build more efficient queries, but it cannot be a part of the public interface.
        /// </remarks>
        /// <typeparam name="T">Aggregate root type</typeparam>
        /// <returns>Mongo-specific interface for building queries</returns>
        public IMongoQueryable<T> Query<T>() where T : IAggregateRoot
        {
            var activeSessionHandle = GetActiveSessionHandle();

            return activeSessionHandle != null
                ? GetCollection<T>().AsQueryable(activeSessionHandle)
                : GetCollection<T>().AsQueryable();
        }

        public async Task DeleteAsync<T>(string id) where T : IAggregateRoot
        {
            var activeSessionHandle = GetActiveSessionHandle();
            var filter = Builders<T>.Filter.Eq(m => m.Id, id);

            if (activeSessionHandle != null)
            {
                await GetCollection<T>().DeleteOneAsync(activeSessionHandle, filter);
            }
            else
            {
                await GetCollection<T>().DeleteOneAsync(filter);
            }
        }

        // Transactions

        /// <summary>
        /// Helper method, which returns an active <see cref="IClientSessionHandle"/> if it exists and has not been disposed yet.
        /// Operations of this class should use this method to pass the session handle down to the Mongo SDK.
        /// </summary>
        /// <returns>
        /// Active session handle, or null if there is none
        /// </returns>
        private IClientSessionHandle GetActiveSessionHandle()
        {
            if (_currentTransaction == null || _currentTransaction.IsDisposed)
                return null;

            return _currentTransaction.SessionHandle;
        }
        
        public async Task<IDatabaseTransaction> StartTransactionAsync()
        {
            if (GetActiveSessionHandle() != null)
                throw new InvalidOperationException("Cannot StartTransactionAsync, already in transaction!");

            var session = await _client.StartSessionAsync();

            _currentTransaction = new MongoSessionTransaction(session);

            return _currentTransaction;
        }
    }
}