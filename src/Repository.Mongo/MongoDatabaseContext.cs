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
    public class MongoDatabaseContext : IDatabaseContext
    {
        private readonly MongoOptions _mongoOptions;

        private readonly ICollectionNameMapper _collectionNameMapper;
        private readonly MongoClient _client;

        public MongoDatabaseContext(IOptions<MongoOptions> mongoOptions, ICollectionNameMapper collectionNameMapper)
        {
            _mongoOptions = mongoOptions.Value;
            _collectionNameMapper = collectionNameMapper;
            _client = new MongoClient(_mongoOptions.ConnectionString);
        }

        internal IMongoCollection<T> GetCollection<T>()
        {
            string collectionName = _collectionNameMapper.GetCollectionName<T>();
            return _client.GetDatabase(_mongoOptions.DatabaseName).GetCollection<T>(collectionName);
        }

        public Task<double?> PingAsync()
        {
            var database = _client.GetDatabase(_mongoOptions.DatabaseName);
            
            DateTime start = DateTime.Now;

            bool pingReceived = database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(5000);

            double? elapsed = pingReceived ? (DateTime.Now - start).TotalMilliseconds : null;

            return Task.FromResult(elapsed);
        }

        public async Task<T> CreateAsync<T>(T model) where T : IAggregateRoot
        {
            await GetCollection<T>().InsertOneAsync(model);
            return model;
        }

        public async Task<T> LoadAsync<T>(string id) where T : IAggregateRoot
        {
            return await GetCollection<T>().AsQueryable().SingleOrDefaultAsync(m => m.Id == id);
        }

        public IQueryable<T> Query<T>() where T : IAggregateRoot
        {
            return GetCollection<T>().AsQueryable();
        }

        public async Task DeleteAsync<T>(string id) where T : IAggregateRoot
        {
            var filter = Builders<T>.Filter.Eq(m => m.Id, id);
            await GetCollection<T>().DeleteOneAsync(filter);
        }
    }
}