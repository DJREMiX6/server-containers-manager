using ServerContainerManager.Application.Models;

namespace ServerContainerManager.Application.Commands.UpdateUserNamespaces
{
    public sealed record UpdateUserNamespacesCommandResult
    {
        public IList<NamespaceInfo> Namespaces { get; }

        public UpdateUserNamespacesCommandResult(IList<NamespaceInfo> namespaces)
        {
            Namespaces = namespaces;
        }
    }
}
