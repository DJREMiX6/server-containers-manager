using ErrorOr;

namespace ServerContainerManager.Domain.Entities.Auth.Errors
{
    internal static class UserValidationErrors
    {
        private const string CodeKey = "UserValidation";

        public static Error UsernameTooShort() => Error.Validation($"{CodeKey}.{nameof(UsernameTooShort)}", "Username must be at least 3 characters long.");
        public static Error EmptyNamespaces() => Error.Validation($"{CodeKey}.{nameof(EmptyNamespaces)}", "Namespaces list cannot be empty.");
        public static Error AlreadyConfirmed(Guid userId) => Error.Validation($"{CodeKey}.{nameof(AlreadyConfirmed)}", $"User {userId} already confirmed");
        public static Error AlreadyNotConfirmed(Guid userId) => Error.Validation($"{CodeKey}.{nameof(AlreadyNotConfirmed)}", $"User {userId} already not confirmed");
    }
}
