using System;
using QuickShop.Repository.Mongo.CollectionNameMapping;

namespace QuickShop.Repository.Mongo.Tests
{
    /// <summary>
    /// Wraps another <see cref="ICollectionNameMapper"/> to pass through base results with some static suffix.
    /// Used in tests to set up new collections and clean them up after all tests are completed.
    /// </summary>
    internal class TestSuffixedCollectionNameMapperWrapper : ICollectionNameMapper
    {
        private readonly string _prefix;
        private readonly ICollectionNameMapper _baseMapper;

        public TestSuffixedCollectionNameMapperWrapper(ICollectionNameMapper baseMapper, string prefix)
        {
            _baseMapper = baseMapper ?? throw new ArgumentNullException(nameof(baseMapper));
            _prefix = prefix ?? throw new ArgumentNullException(nameof(prefix));
        }

        public string GetCollectionName<T>()
        {
            return _prefix + _baseMapper.GetCollectionName<T>();
        }
    }
}