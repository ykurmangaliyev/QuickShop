using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.Model;
using Microsoft.AspNetCore.Mvc;
using QuickShop.Domain.Accounts.Authentication;

namespace API.Controllers
{
    [Route("api/ping")]
    [ApiController]
    public class PingController : ControllerBase
    {
        // POST api/ping
        [HttpGet]
        public async Task<ActionResult<PingResponse>> Get()
        {
            return Ok(PingResponse.Create());
        }
    }
}
