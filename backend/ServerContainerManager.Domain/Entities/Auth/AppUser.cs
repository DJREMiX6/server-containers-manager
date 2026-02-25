using ErrorOr;
using Microsoft.AspNetCore.Identity;
using ServerContainerManager.Domain.Entities.Namespaces;

namespace ServerContainerManager.Domain.Entities.Auth
{
    public sealed class AppUser : IdentityUser<Guid>
    {
        private List<Namespace> _namespaces;

        public IReadOnlyList<Namespace> Namespaces => _namespaces;

        private AppUser() { } // EF

        private AppUser(string username, IEnumerable<Namespace> namespaces) : base(username)
        {
            _namespaces = [.. namespaces];
        }

        // TODO: Implement ErrorOr result with Domain validation
        public static AppUser Create(string username, IEnumerable<Namespace> namespaces)
        {
            return new AppUser(username, namespaces);
        }

        public ErrorOr<Success> UpsertNamespaces(IList<Namespace> namespaces)
        {
            _namespaces = [.. namespaces];

            return Result.Success;
        }
    }
}
