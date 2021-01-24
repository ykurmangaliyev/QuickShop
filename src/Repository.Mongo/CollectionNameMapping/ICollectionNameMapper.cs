namespace QuickShop.Repository.Mongo.CollectionNameMapping
{
    public interface ICollectionNameMapper
    {
        string GetCollectionName<T>();
    }
}