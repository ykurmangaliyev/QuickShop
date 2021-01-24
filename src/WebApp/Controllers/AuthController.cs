using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<AuthenticateResponse>> SignIn(AuthenticateRequest request)
        {
            var authAttempt = await _userAuthService.AuthenticateAsync(request.Username, request.Password);

            if (!authAttempt.IsSuccess)
            {
                return StatusCode((int) HttpStatusCode.Unauthorized, new AuthenticateResponse
                {
                    ResultCode = authAttempt.Code.ToString("G"),
                });
            }

            Response.Cookies.Append(
                JwtBearerAuthenticationOptions.JwtBearerAuthentication,
                _jwtTokenGenerator.CreateToken(authAttempt.User),
                new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(7),
                    HttpOnly = false,
                }
            );

            return Ok(new AuthenticateResponse
            {
                ResultCode = authAttempt.Code.ToString("G"),
            });
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public new ActionResult SignOut()
        {
            Response.Cookies.Delete(JwtBearerAuthenticationOptions.JwtBearerAuthentication);
            return Ok();
        }
    }
}