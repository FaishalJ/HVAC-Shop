using System.Security.Claims;

namespace HVAC_Shop.Core.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetName(this ClaimsPrincipal user)
        {
            return user.Identity?.Name ?? throw new UnauthorizedAccessException("User is not logged in.");
        }
    }
}
