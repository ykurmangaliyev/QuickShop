using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/ping")]
    [Authorize]
    public class PingController : Controller
    {
        // POST /api/ping
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult Ping()
        {
            return Ok(new {
                ServerTime = DateTime.UtcNow.ToString("O"),
                UserId = User.Claims.SingleOrDefault(c => c.Type == CustomClaimTypes.Id)?.Value,
            });
        }
    }
}