using Microsoft.AspNetCore.Identity;

namespace ServerContainerManager.Domain.Entities.Auth
{
    public sealed class AppRole : IdentityRole<Guid>
    {
        public AppRole() : base() { } // EF

        public AppRole(string roleName) : base(roleName) { }
    }
}
