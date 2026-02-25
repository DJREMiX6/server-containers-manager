using System.Security.Claims;

namespace ServerContainerManager.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool TryGetUserId(this ClaimsPrincipal claims, out Guid userId)
        {
            var userIdClaim = claims.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                userId = Guid.Empty;
                return false;
            }

            var isParsed = Guid.TryParse(userIdClaim, out userId);
            if (!isParsed)
                return false;

            return true;
        }

        public static Guid GetUserId(this ClaimsPrincipal claims)
        {
            var userIdFound = claims.TryGetUserId(out var userId);
            if (!userIdFound)
                throw new ArgumentException("UserId not found in claims", nameof(claims));

            return userId;
        }
    }
}
