namespace ServerContainerManager.Application.Models.Enums
{
    public enum ContainerState
    {
        Created,
        Running,
        Paused,
        Restarting,
        Exited,
        Removing,
        Dead,
    }

    internal static class ContainerStateHelper
    {
        public static ContainerState FromDomain(Domain.Entities.Containers.Enums.ContainerState state) =>
        state switch
        {
            Domain.Entities.Containers.Enums.ContainerState.Created => ContainerState.Created,
            Domain.Entities.Containers.Enums.ContainerState.Running => ContainerState.Running,
            Domain.Entities.Containers.Enums.ContainerState.Paused => ContainerState.Paused,
            Domain.Entities.Containers.Enums.ContainerState.Restarting => ContainerState.Restarting,
            Domain.Entities.Containers.Enums.ContainerState.Exited => ContainerState.Exited,
            Domain.Entities.Containers.Enums.ContainerState.Removing => ContainerState.Removing,
            Domain.Entities.Containers.Enums.ContainerState.Dead => ContainerState.Dead,
            _ => throw new ArgumentException($"Invalid Container State {state}", nameof(state)),
        };
    
    }
}
