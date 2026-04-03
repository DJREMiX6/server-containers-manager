using ServerContainerManager.Application.Models;

namespace ServerContainerManager.Application.Commands.User.UpdateUserNamespaces
{
    public sealed record UpdateUserNamespacesCommandResult
    {
        public required IList<NamespaceInfo> Namespaces { get; init; }
    }
}
