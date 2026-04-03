using ErrorOr;

namespace ServerContainerManager.Domain.Entities.Containers.Errors
{
    internal static class ContainerValidationErrors
    {
        private const string CodeKey = "ContainerValidation";

        public static Error NameTooShort() => Error.Validation($"{CodeKey}.{nameof(NameTooShort)}", "Name must be at least 3 characters long.");
        public static Error InvalidState() => Error.Validation($"{CodeKey}.{nameof(InvalidState)}", "State must be a valid container state.");
        public static Error NullOrEmptyId() => Error.Validation($"{CodeKey}.{nameof(NullOrEmptyId)}", "Docker container ID cannot be null or empty.");
        public static Error InvalidIdLength() => Error.Validation($"{CodeKey}.{nameof(InvalidIdLength)}", "Docker container ID must be 64 characters long.");
        public static Error InvalidIdFormat() => Error.Validation($"{CodeKey}.{nameof(InvalidIdFormat)}", "Invalid Docker container ID format.");
        public static Error EmptyNamespaces() => Error.Validation($"{CodeKey}.{nameof(EmptyNamespaces)}", "Namespaces list cannot be empty.");
    }
}
