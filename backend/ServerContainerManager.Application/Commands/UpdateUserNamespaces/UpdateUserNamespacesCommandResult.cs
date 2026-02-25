using ServerContainerManager.Application.Commands.Models;

namespace ServerContainerManager.Application.Commands.UpdateUserNamespaces
{
    public record UpdateUserNamespacesCommandResult
    {
        public IList<NamespaceInfo> Namespaces { get; }

        public UpdateUserNamespacesCommandResult(IList<NamespaceInfo> namespaces)
        {
            Namespaces = namespaces;
        }
    }
}
