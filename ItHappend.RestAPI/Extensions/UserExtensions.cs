using System;
using System.Security.Claims;
using ItHappend.RestAPI.Authentication;

namespace ItHappend.RestAPI.Extensions
{
    public static class UserExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirstValue(JwtClaimTypes.Id));
        }
    }
}