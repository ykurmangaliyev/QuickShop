using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuickShop.Domain.Accounts.Model.UserAggregate;

namespace QuickShop.WebApp.Authentication
{
    public class JwtTokenGenerator
    {
        private readonly JwtBearerAuthenticationOptions _jwtOptions;

        public JwtTokenGenerator(IOptions<JwtBearerAuthenticationOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string CreateToken(User user)
        {
            byte[] key = Encoding.UTF8.GetBytes(_jwtOptions.SymmetricKey);
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(CustomClaimTypes.Id, user.Id),
                    new Claim(ClaimTypes.NameIdentifier, user.Credentials.Username),
                }),
                
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.Add(TimeSpan.FromSeconds(_jwtOptions.ExpirationTimeInSeconds)),
                
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
