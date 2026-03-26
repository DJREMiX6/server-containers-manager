using ErrorOr;
using Microsoft.AspNetCore.Identity;
using ServerContainerManager.Domain.Entities.Auth.Errors;
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

        public static ErrorOr<AppUser> Create(string username, IEnumerable<Namespace> namespaces)
        {
            var errors = new List<Error>();

            if (string.IsNullOrEmpty(username) || username.Length < 3)
                errors.Add(Error.Validation($"{nameof(AppUser)}.{nameof(Create)}", "Username must be at least 3 characters long"));

            if (errors.Count > 0)
                return errors;

            return new AppUser(username, namespaces);
        }

        public ErrorOr<Success> UpsertNamespaces(IList<Namespace> namespaces)
        {
            if (namespaces.Count == 0)
                return UserValidationErrors.EmptyNamespaces();

            _namespaces = [.. namespaces];

            return Result.Success;
        }
    }
}
