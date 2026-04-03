namespace ServerContainerManager.Domain.Entities.Containers.Enums
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
}
