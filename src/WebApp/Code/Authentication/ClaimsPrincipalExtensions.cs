using System;
using System.Linq;
using System.Security.Claims;

namespace QuickShop.WebApp.Authentication
{
    public static class ClaimsPrincipalExtensions
    {
        public static string Id(this ClaimsPrincipal principal)
        {
            if (principal == null) 
                throw new ArgumentNullException(nameof(principal));

            if (principal.Identity == null || !principal.Identity.IsAuthenticated)
                throw new InvalidOperationException("Cannot use .Id() on unauthorized principal");

            return principal.Claims.First(c => c.Type == CustomClaimTypes.Id).Value;
        }
    }
}