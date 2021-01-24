using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuickShop.Domain.Abstractions;

namespace QuickShop.Repository.Abstractions
{
    /// <summary>
    /// Should be primarily used for integration tests
    /// </summary>
    public class InMemoryDatabaseContext : IDatabaseContext
    {
        private readonly IDictionary<Type, object> dictionary = new Dictionary<Type, object>();

        private IList<T> GetList<T>()
        {
            if (dictionary.TryGetValue(typeof(T), out object foundList))
            {
                return (IList<T>)foundList;
            }
            else
            {
                List<T> newList = new List<T>();

                dictionary[typeof(T)] = newList;

                return newList;
            }
        }
        
        public Task<T> CreateAsync<T>(T model) where T : IAggregateRoot
        {
            GetList<T>().Add(model);
            return Task.FromResult(model);
        }

        public Task<T> LoadAsync<T>(string id) where T : IAggregateRoot
        {
            return Task.FromResult(GetList<T>().SingleOrDefault(x => x.Id == id));
        }

        public IQueryable<T> Query<T>() where T : IAggregateRoot
        {
            return GetList<T>().AsQueryable();
        }

        public Task DeleteAsync<T>(string id) where T : IAggregateRoot
        {
            var found = GetList<T>().Single(x => x.Id == id);

            GetList<T>().Remove(found);

            return Task.CompletedTask;
        }
    }
}