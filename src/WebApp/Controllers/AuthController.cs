using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickShop.Domain.Accounts;
using QuickShop.Domain.Accounts.Authentication;
using WebApp.Authentication;
using WebApp.Model;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserAuthService _userAuthService;

        public AuthController(JwtTokenGenerator jwtTokenGenerator, IUserAuthService userAuthService)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userAuthService = userAuthService;
        }

        // POST /api/auth
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<AuthenticateResponse>> Authenticate(AuthenticateRequest request)
        {
            var authAttempt = await _userAuthService.AuthenticateAsync(request.Username, request.Password);

            if (!authAttempt.IsSuccess)
            {
                return StatusCode((int) HttpStatusCode.Unauthorized, new AuthenticateResponse
                {
                    ResultCode = authAttempt.Code.ToString("G"),
                });
            }

            return Ok(new AuthenticateResponse
            {
                ResultCode = authAttempt.Code.ToString("G"),
                Token = _jwtTokenGenerator.CreateToken(authAttempt.User),
            });
        }
    }
}