namespace QuickShop.Repository.Mongo.CollectionNameMapping
{
    public class TypeCollectionNameMapper : ICollectionNameMapper
    {
        public string GetCollectionName<T>()
        {
            return typeof(T).Name.ToLowerInvariant();
        }
    }
}