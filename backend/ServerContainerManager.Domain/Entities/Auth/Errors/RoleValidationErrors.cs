using ErrorOr;

namespace ServerContainerManager.Domain.Entities.Auth.Errors
{
    internal static class RoleValidationErrors
    {
        private const string CodeKey = "RoleValidation";

        public static Error NameTooShort() => Error.Validation($"{CodeKey}.{nameof(NameTooShort)}", "Role name must be at least 3 characters long");
    }
}
