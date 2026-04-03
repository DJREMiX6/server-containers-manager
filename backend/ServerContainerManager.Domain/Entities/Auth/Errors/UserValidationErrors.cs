using ErrorOr;

namespace ServerContainerManager.Domain.Entities.Auth.Errors
{
    internal static class UserValidationErrors
    {
        private const string CodeKey = "UserValidation";

        public static Error UsernameTooShort() => Error.Validation($"{CodeKey}.{nameof(UsernameTooShort)}", "Username must be at least 3 characters long.");
        public static Error EmptyNamespaces() => Error.Validation($"{CodeKey}.{nameof(EmptyNamespaces)}", "Namespaces list cannot be empty.");
    }
}
