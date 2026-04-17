using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ServerContainerManager.Domain.Entities.Auth;
using System.Security.Claims;

namespace ServerContainerManager.Application.Identity
{
    internal class AppUserClaimsPrincipalFactory(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        IOptions<IdentityOptions> options) : UserClaimsPrincipalFactory<AppUser, AppRole>(userManager, roleManager, options)
    {
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            identity.AddClaim(new Claim(
            UserClaims.IsUserConfirmed,
            user.IsConfirmed.ToString().ToLowerInvariant()));

            return identity;
        }
    }
}
