namespace ServerContainerManager.Application.Commands.Models
{
    public record NamespaceInfo
    {
        public Guid Id { get; }
        public string Name { get; }

        public NamespaceInfo(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
