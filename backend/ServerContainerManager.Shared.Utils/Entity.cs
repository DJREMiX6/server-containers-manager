namespace ServerContainerManager.Shared.Utils
{
    public abstract class Entity<TId>
    {
        public TId Id { get; protected set; }
    }
}
