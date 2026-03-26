using ErrorOr;

namespace ServerContainerManager.Shared.Utils.Errors
{
    public static class ContainerErrors
    {
        public static Error NotFound(string containerId) => Error.NotFound("Container.NotFound", $"Cannot find container {containerId}.");
        public static Error ContainerNotRunning(string containerId) => Error.Conflict("Container.ContainerNotRunning", $"The container {containerId} is not running.");
    }
}
