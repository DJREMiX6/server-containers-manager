using ErrorOr;

namespace ServerContainerManager.Shared.Utils.Errors
{
    public static class NamespaceErrors
    {
        private const string CodeKey = "Namespace";

        public static Error AlreadyExists(string namespaceName) => Error.Conflict($"{CodeKey}.{nameof(AlreadyExists)}", $"Namespace with name {namespaceName} already exists.");
        public static Error SomeNotExist(IEnumerable<Guid> namespacesIds) => Error.Validation($"{CodeKey}.{nameof(SomeNotExist)}", $"The namespaces {string.Join(", ", namespacesIds)} do not exist.");
        public static Error NotFound(Guid namespaceId) => Error.NotFound($"{CodeKey}.{nameof(NotFound)}", $"The namespace with id {namespaceId} does not exist.");
    }
}
