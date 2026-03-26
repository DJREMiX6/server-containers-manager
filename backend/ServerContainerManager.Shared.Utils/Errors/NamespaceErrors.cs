using ErrorOr;

namespace ServerContainerManager.Shared.Utils.Errors
{
    public static class NamespaceErrors
    {
        public static Error AlreadyExists(string namespaceName) => Error.Conflict("Namespace.AlreadyExists", $"Namespace with name {namespaceName} already exists.");
        public static Error SomeNotExist(IEnumerable<Guid> namespacesIds) => Error.Validation("Namespace.SomeNotExists", $"The namespaces {string.Join(", ", namespacesIds)} do not exist.");
    }
}
