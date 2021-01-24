using System;
using QuickShop.Repository.Mongo.CollectionNameMapping;

namespace QuickShop.Repository.Mongo.Tests
{
    internal class CodedCollectionNameMapper : ICollectionNameMapper
    {
        private readonly Func<Type, string> _func;

        public CodedCollectionNameMapper(Func<Type, string> func)
        {
            _func = func;
        }

        public string GetCollectionName<T>()
        {
            return _func(typeof(T));
        }
    }
}