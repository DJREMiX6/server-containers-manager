namespace ServerContainerManager.API.Models.Responses.Models
{
    public record NamespaceInfoResponseModel
    {
        public Guid Id { get; }
        public string Name { get; }

        public NamespaceInfoResponseModel(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
