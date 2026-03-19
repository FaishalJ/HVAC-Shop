using System.Security.Claims;

namespace HVAC_Shop.Core.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        extension(ClaimsPrincipal user)
        {
            public string GetName()
            {
                return user.Identity?.Name ?? throw new UnauthorizedAccessException("User is not logged in.");
            }
        }
    }
}
