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
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthService _userAuthService;

        public AuthController(IUserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
        }

        // POST api/auth
        [HttpPost]
        public async Task<ActionResult<AuthPostResponse>> Post([FromBody] AuthPostRequest request)
        {
            var authenticationResult = await _userAuthService.AuthenticateAsync(request.Username, request.Password);

            if (!authenticationResult.IsSuccess)
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, new ErrorResponse
                {
                    ErrorMessage = authenticationResult.Code.ToString(),
                });
            }

            return Ok(new AuthPostResponse
            {
                UserToken = authenticationResult.User.Token
            });
        }
    }
}
