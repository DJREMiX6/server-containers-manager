using ErrorOr;

namespace ServerContainerManager.Shared.Utils.Errors
{
    public static class ContainerErrors
    {
        private const string CodeKey = "Container";

        public static Error NotFound(string containerId) => Error.NotFound($"{CodeKey}.{nameof(NotFound)}", $"Cannot find container {containerId}.");
        public static Error NotRunning(string containerId) => Error.Conflict($"{CodeKey}.{nameof(NotRunning)}", $"The container {containerId} is not running.");
        public static Error NotPaused(string containerId) => Error.Conflict($"{CodeKey}.{nameof(NotPaused)}", $"The container {containerId} is not paused.");
        public static Error AlreadyRunning(string containerId) => Error.Conflict($"{CodeKey}.{nameof(AlreadyRunning)}", $"The container {containerId} is already running.");
        public static Error Removing(string containerId) => Error.Conflict($"{CodeKey}.{nameof(Removing)}", $"The container {containerId} is being removed.");
        public static Error NotFoundList(ICollection<string> containerIds) => Error.Validation($"{CodeKey}.{nameof(NotFoundList)}", $"Containers {string.Join(", ", containerIds)} not found.");
    }
}
