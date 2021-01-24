namespace QuickShop.Repository.Mongo.Configuration
{
    public class MongoOptions
    {
        public const string Mongo = nameof(Mongo);

        public string ConnectionString { get; set;  }

        public string DatabaseName { get; set; }
    }
}
