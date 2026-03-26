using ErrorOr;

namespace ServerContainerManager.Domain.Entities.Namespaces.Errors
{
    internal static class NamespaceValidationErrors
    {
        private const string CodeKey = "NamespaceValidation";

        public static Error NameTooShort() => Error.Validation($"{CodeKey}.{nameof(NameTooShort)}", "Namespace name must be at least 3 characters long."); 
    }
}
