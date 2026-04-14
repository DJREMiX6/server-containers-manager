using ErrorOr;

namespace ServerContainerManager.Domain.Entities.Auth.Errors
{
    internal static class UserValidationErrors
    {
        private const string CodeKey = "UserValidation";
        private const string UsernameTooShortCode = $"{CodeKey}.{nameof(UsernameTooShort)}";
        private const string EmptyNamespacesCode = $"{CodeKey}.{nameof(EmptyNamespaces)}";
        private const string AlreadyConfirmedCode = $"{CodeKey}.{nameof(AlreadyConfirmed)}";
        private const string AlreadyNotConfirmedCode = $"{CodeKey}.{nameof(AlreadyNotConfirmed)}";

        public static Error UsernameTooShort() => Error.Validation(UsernameTooShortCode, "Username must be at least 3 characters long.");
        public static Error EmptyNamespaces() => Error.Validation(EmptyNamespacesCode, "Namespaces list cannot be empty.");
        public static Error AlreadyConfirmed(Guid userId) => Error.Validation(AlreadyConfirmedCode, $"User {userId} already confirmed");
        public static Error AlreadyNotConfirmed(Guid userId) => Error.Validation(AlreadyNotConfirmedCode, $"User {userId} already not confirmed");
    }
}
