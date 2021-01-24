using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using QuickShop.Domain.Abstractions;

namespace QuickShop.Repository.Abstractions
{
    public interface IDatabaseContext
    {
        Task<T> CreateAsync<T>(T model) where T : IAggregateRoot;

        Task<T> LoadAsync<T>(string id) where T : IAggregateRoot;

        IQueryable<T> Query<T>() where T : IAggregateRoot;

        Task DeleteAsync<T>(string id) where T : IAggregateRoot;
    }
}
