using MongoDB.Driver;

namespace QuickShop.Repository.Mongo
{
    public class MongoClientWrapper
    {
        private readonly MongoClient _client;
        private readonly string _databaseName;

        public MongoClientWrapper(MongoClient client, string databaseName)
        {
            _client = client;
            _databaseName = databaseName;
        }

        public IMongoDatabase GetDatabase()
        {
            return _client.GetDatabase(_databaseName);
        }
    }
}