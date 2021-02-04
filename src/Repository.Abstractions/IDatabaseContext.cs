using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using QuickShop.Domain.Abstractions;

namespace QuickShop.Repository.Abstractions
{
    public interface IDatabaseContext
    {
        Task<double?> PingAsync();

        Task<T> CreateAsync<T>(T model) where T : IAggregateRoot;

        /// <summary>
        /// Asynchronously load an entity by its unique ID token
        /// </summary>
        /// <typeparam name="T">Aggregate root type</typeparam>
        /// <param name="id">Unique ID token of an aggregate root</param>
        /// <returns>
        /// Returns null if not found
        /// </returns>
        Task<T> LoadAsync<T>(string id) where T : IAggregateRoot;

        IQueryable<T> Query<T>() where T : IAggregateRoot;

        Task DeleteAsync<T>(string id) where T : IAggregateRoot;

        Task<IDatabaseTransaction> StartTransactionAsync();
    }
}
