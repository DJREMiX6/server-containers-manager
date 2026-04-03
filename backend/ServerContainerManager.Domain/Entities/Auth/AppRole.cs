using ErrorOr;
using Microsoft.AspNetCore.Identity;
using ServerContainerManager.Domain.Entities.Auth.Errors;

namespace ServerContainerManager.Domain.Entities.Auth
{
    public sealed class AppRole : IdentityRole<Guid>
    {
        private AppRole() : base() { } // EF

        private AppRole(string name) : base(name) { }

        public static ErrorOr<AppRole> Create(string name)
        {
            name = name.Trim();

            if (string.IsNullOrEmpty(name) || name.Length < 3)
                return RoleValidationErrors.NameTooShort();

            return new AppRole(name);
        }
    }
}
