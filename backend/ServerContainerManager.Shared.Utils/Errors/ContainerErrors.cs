using ErrorOr;

namespace ServerContainerManager.Shared.Utils.Errors
{
    public static class ContainerErrors
    {
        public static Error ContainerNotRunning(string containerId) => Error.Conflict("Container.ContainerNotRunning", $"The container {containerId} is not running.");
    }
}
