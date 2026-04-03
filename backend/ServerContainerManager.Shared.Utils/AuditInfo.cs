namespace ServerContainerManager.Shared.Utils
{
    public class AuditInfo
    {
        public required DateTime At { get; init; }
        public required Actor By { get; init; }
    }
}
