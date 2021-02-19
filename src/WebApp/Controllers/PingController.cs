using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuickShop.Domain.Ping;
using QuickShop.WebApp.Model;

namespace QuickShop.WebApp.Controllers
{
    [ApiController]
    [Route("api/ping")]
    public class PingController : Controller
    {
        private readonly IPingService _pingService;

        public PingController(IPingService pingService)
        {
            _pingService = pingService;
        }

        // POST /api/ping
        [HttpPost]
        public async Task<ActionResult<PingResponse>> Ping()
        {
            var response = await _pingService.Ping();

            return Ok(new PingResponse
            {
                DatabasePing = response.DatabasePing,
                DatabaseStatus = response.DatabaseStatus,
                ServerTime = response.ServerTime
            });
        }
    }
}