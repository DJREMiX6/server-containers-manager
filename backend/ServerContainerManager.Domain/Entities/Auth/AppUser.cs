using Microsoft.AspNetCore.Identity;
using ServerContainerManager.Domain.Entities.Namespaces;

namespace ServerContainerManager.Domain.Entities.Auth
{
    public sealed class AppUser : IdentityUser<Guid>
    {
        private readonly List<Namespace> _namespaces;

        public IEnumerable<Namespace> Namespaces => _namespaces;

        private AppUser() { } // EF

        private AppUser(string username, IEnumerable<Namespace> namespaces) : base(username)
        {
            _namespaces = [.. namespaces];
        }

        // TODO: Implemente ErrorOr result with Domain validation
        public static AppUser Create(string username, IEnumerable<Namespace> namespaces)
        {
            return new AppUser(username, namespaces);
        }
    }
}
