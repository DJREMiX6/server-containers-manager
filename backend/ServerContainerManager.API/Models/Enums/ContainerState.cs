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

    internal static class ContainerStateHelper
    {
        public static ContainerState FromApplication(Application.Models.Enums.ContainerState state) =>
        state switch
        {
            Application.Models.Enums.ContainerState.Created => ContainerState.Created,
            Application.Models.Enums.ContainerState.Running => ContainerState.Running,
            Application.Models.Enums.ContainerState.Paused => ContainerState.Paused,
            Application.Models.Enums.ContainerState.Restarting => ContainerState.Restarting,
            Application.Models.Enums.ContainerState.Exited => ContainerState.Exited,
            Application.Models.Enums.ContainerState.Removing => ContainerState.Removing,
            Application.Models.Enums.ContainerState.Dead => ContainerState.Dead,
            _ => throw new ArgumentException($"Invalid Container State {state}", nameof(state)),
        };
    }
}
