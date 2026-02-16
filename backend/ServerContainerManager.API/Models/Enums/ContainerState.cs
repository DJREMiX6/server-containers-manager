using ServerContainerManager.API.Consts;

namespace ServerContainerManager.API.Models.Enums
{
    public enum ContainerState
    {
        Created,
        Running,
        Paused,
        Restarting,
        Exited,
        Removing,
        Dead
    }

    public static class ContainerStateHelper
    {
        public static ContainerState FromDockerApiStatus(string status) =>
        status switch
        {
            DockerApiState.Created => ContainerState.Created,
            DockerApiState.Running => ContainerState.Running,
            DockerApiState.Paused => ContainerState.Paused,
            DockerApiState.Restarting => ContainerState.Restarting,
            DockerApiState.Exited => ContainerState.Exited,
            DockerApiState.Removing => ContainerState.Removing,
            DockerApiState.Dead => ContainerState.Dead,
            _ => throw new ArgumentException($"Invalid status {status}", nameof(status)),
        };
    }
}
