namespace ServerContainerManager.Application.Commands.GetContainerList
{
    public record GetContainerListCommandResultContainerInfo
    {
        public string Id { get; }
        public string Name { get; }
        public string Status { get; }
        public DateTime Created { get; }
        public IDictionary<string, string> Labels { get; }
        public IList<ushort> PrivatePorts { get; }
        public IList<ushort> PublicPorts { get; }

        public GetContainerListCommandResultContainerInfo(
            string id,
            string name,
            string status,
            DateTime created,
            IDictionary<string, string> labels,
            IList<ushort> privatePorts,
            IList<ushort> publicPorts)
        {
            Id = id;
            Name = name;
            Status = status;
            Created = created;
            Labels = labels;
            PrivatePorts = privatePorts;
            PublicPorts = publicPorts;
        }
    }

    public record GetContainerListCommandResult
    {
        public IReadOnlyList<GetContainerListCommandResultContainerInfo> Containers { get; }

        public GetContainerListCommandResult(IList<GetContainerListCommandResultContainerInfo> containers)
        {
            Containers = [.. containers];
        }
    }
}
