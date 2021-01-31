using System;
using System.Threading.Tasks;
using QuickShop.Domain.Ping.Model;
using QuickShop.Repository.Abstractions;

namespace QuickShop.Domain.Ping
{
    public interface IPingService
    {
        Task<PingCheck> Ping();
    }

    public class PingService : IPingService
    {
        private readonly IDatabaseContext _databaseContext;

        public PingService(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<PingCheck> Ping()
        {
            double? databasePing = await _databaseContext.PingAsync();

            return new PingCheck
            {
                ServerTime = DateTimeOffset.Now,
                DatabasePing = databasePing,
                DatabaseStatus = databasePing != null,
            };
        }
    }
}
