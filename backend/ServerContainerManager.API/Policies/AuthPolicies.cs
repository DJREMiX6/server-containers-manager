using Microsoft.AspNetCore.Authorization;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Application.Identity;

namespace ServerContainerManager.API.Policies
{
    public static class AuthPolicies
    {
        public static class AuthenticatedUserPolicy
        {
            public const string Name = "AuthenticatedUser";
            public readonly static AuthorizationPolicy Policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        }

        public static class ConfirmedUserPolicy
        {
            public const string Name = "ConfirmedUser";
            public readonly static AuthorizationPolicy Policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(UserClaims.IsUserConfirmed, "true")
                .Build();
        }

        public static class UnconfirmedUserPolicy
        {
            public const string Name = "UnconfirmedUser";
            public readonly static AuthorizationPolicy Policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(UserClaims.IsUserConfirmed, "false")
                .Build();
        }

        public static class ConfirmedAdminPolicy
        {
            public const string Name = "ConfirmedAdmin";
            public readonly static AuthorizationPolicy Policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(UserClaims.IsUserConfirmed, "true")
                .RequireRole(UserRoles.Admin)
                .Build();
        }
    }
}
