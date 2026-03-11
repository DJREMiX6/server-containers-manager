using ServerContainerManager.Application.Models;

namespace ServerContainerManager.Application.Queries.GetContainerList
{
    public sealed record GetContainerListQuery
    {
        public required Guid UserId { get; init; }
        public required int Skip { get; init; }
        public required int Take { get; init; }
        public required ContainerSortBy SortBy { get; init; }
        public required SortOrder Order { get; init; }
    }
}
