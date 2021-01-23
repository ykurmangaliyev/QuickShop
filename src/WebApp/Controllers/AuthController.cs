using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/auth")]
    //[Authorize]
    public class AuthController : Controller
    {
        // GET
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("test");
        }
    }
}