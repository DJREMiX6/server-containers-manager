using ServerContainerManager.API.Models.Enums;

namespace ServerContainerManager.API.Models.Responses.ContainersController
{
    public record GetContainerListItemResponse
    {
        public string Id { get; }
        public string Name { get; }
        public ContainerState State { get; }
        public DateTime Created { get; }
        public IDictionary<string, string> Labels { get; }
        public IList<ushort> PublicPorts { get; }

        public GetContainerListItemResponse(
            string id,
            string name,
            ContainerState state,
            DateTime created,
            IDictionary<string, string> labels,
            IList<ushort> publicPorts)
        {
            Id = id;
            Name = name;
            State = state;
            Created = created;
            Labels = labels;
            PublicPorts = [.. publicPorts];
        }
    }

    public record GetContainerListResponse
    {
        public IList<GetContainerListItemResponse> Items { get; }

        public GetContainerListResponse(IList<GetContainerListItemResponse> items)
        {
            Items = [.. items];
        }
    }
}
