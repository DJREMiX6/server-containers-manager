namespace ServerContainerManager.Shared.Utils.Abstraction
{
    public interface IAuditableEntity
    {
        public AuditInfo Created { get; }
        public AuditInfo Updated { get; }
    }
}
