namespace ServerContainerManager.Application.Queries.GetContainerList
{
    public record GetContainerListQueryResultContainerInfo
    {
        public required string Id { get; init; }
        public required string Name { get; init; }
        public required string Status { get; init; }
        public required DateTime Created { get; init; }
        public required IDictionary<string, string> Labels { get; init; }
        public required IList<ushort> PrivatePorts { get; init; }
        public required IList<ushort> PublicPorts { get; init; }
    }

    public record GetContainerListQueryResult
    {
        public required IReadOnlyList<GetContainerListQueryResultContainerInfo> Containers { get; init; }
        public required int TotalCount { get; init; }
    }
}
