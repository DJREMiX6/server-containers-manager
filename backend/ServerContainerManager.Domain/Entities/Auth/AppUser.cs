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
        public bool IsConfirmed { get; private set; }
        public DateTime? LastLoginDate { get; private set; }

        private AppUser() { } // EF

        private AppUser(string username, IEnumerable<Namespace> namespaces) : base(username)
        {
            _namespaces = [.. namespaces];
            IsConfirmed = false;
            LastLoginDate = null;
        }

        public static ErrorOr<AppUser> Create(string username, IEnumerable<Namespace> namespaces)
        {
            if (string.IsNullOrEmpty(username) || username.Length < 3)
                return UserValidationErrors.UsernameTooShort();

            return new AppUser(username, namespaces);
        }

        public ErrorOr<Success> UpdateNamespaces(IList<Namespace> namespaces)
        {
            if (namespaces.Count == 0)
                return UserValidationErrors.EmptyNamespaces();

            _namespaces = [.. namespaces];

            return Result.Success;
        }

        public ErrorOr<Success> Confirm()
        {
            if (IsConfirmed)
                return UserValidationErrors.AlreadyConfirmed(Id);

            IsConfirmed = true;
            return Result.Success;
        }

        public ErrorOr<Success> Unconfirm()
        {
            if (!IsConfirmed)
                return UserValidationErrors.AlreadyNotConfirmed(Id);

            IsConfirmed = false;
            return Result.Success;
        }

        public ErrorOr<Success> UpdateLastLogin(DateTime lastLoginDate)
        {
            if(lastLoginDate < LastLoginDate)
                return UserValidationErrors.InvalidDate();

            LastLoginDate = lastLoginDate;
            return Result.Success;
        }
    }
}
