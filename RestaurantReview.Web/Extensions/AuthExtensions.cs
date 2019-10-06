using System;
using System.Linq;
using System.Security.Claims;

namespace RestaurantReview.Web.Extensions
{
    public static class AuthExtensions
    {
        public static bool HasRightsToPerformAction(this ClaimsPrincipal principal, string userId, string[] allowedRoles)
        {
            // If requested user is the logged in user he has no limitations otherwise, check his roles
            return userId == principal.GetUserId() || allowedRoles.Any(principal.IsInRole);
        }

        public static string GetUserId(this ClaimsPrincipal principal)
        {
            var userIdClaims = principal.FindAll(ClaimTypes.NameIdentifier);

            foreach (var claim in userIdClaims)
            {
                if (Guid.TryParse(claim.Value, out _))
                    return claim.Value;
            }

            throw new Exception("User ID not found in claims list.");
        }
    }
}
