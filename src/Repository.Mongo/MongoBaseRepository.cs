using System.Linq;
using System.Threading.Tasks;
using Domain;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace QuickShop.Repository.Mongo
{
    public abstract class MongoBaseRepository<T> where T : IAggregateRoot
    {
        private readonly IMongoCollection<T> _collection;

        protected MongoBaseRepository(MongoClientWrapper client, string collectionName)
        {
            _collection = client.GetDatabase().GetCollection<T>(collectionName);
        }

        // Make it protected
        public IMongoQueryable<T> Query()
        {
            return _collection.AsQueryable();
        }

        public async Task<T> FindByTokenOrDefaultAsync(string token)
        {
            return await _collection.AsQueryable().Where(m => m.Token == token).SingleAsync();
        }

        public async Task<T> CreateAsync(T model)
        {
            await _collection.InsertOneAsync(model);

            return model;
        }

        public async Task DeleteAsync(string token)
        {
            var filter = Builders<T>.Filter.Eq(m => m.Token, token);
            await _collection.DeleteOneAsync(filter);
        }
    }
}