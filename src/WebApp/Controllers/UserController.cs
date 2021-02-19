using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickShop.Domain.Accounts.Authentication;
using QuickShop.WebApp.Authentication;
using QuickShop.WebApp.Model;

namespace QuickShop.WebApp.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET /api/user/current
        [HttpGet("current")]
        public async Task<ActionResult<UserApiModel>> GetCurrentUser()
        {
            string id = User.Id();
            var user = await _userService.FindUserByIdOrDefaultAsync(id);

            return Ok(new UserApiModel
            {
                Username = user.Credentials.Username,
            });
        }
    }
}