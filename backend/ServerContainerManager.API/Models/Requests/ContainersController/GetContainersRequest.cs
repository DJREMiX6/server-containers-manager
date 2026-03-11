using FluentValidation;
using ServerContainerManager.Application.Models;
using ServerContainerManager.Application.Queries.GetContainerList;

namespace ServerContainerManager.API.Models.Requests.ContainersController
{
    public sealed record GetContainersRequest
    {
        public int Skip { get; init; } = 0;
        public int Take { get; init; } = 25;
        public ContainerSortBy SortBy { get; init; } = ContainerSortBy.Name;
        public SortOrder Order { get; init; } = SortOrder.Asc;
    }

    public sealed class GetContainersRequestValidator : AbstractValidator<GetContainersRequest>
    {
        public GetContainersRequestValidator()
        {
            RuleFor(r => r.Skip)
                .GreaterThanOrEqualTo(0);
            RuleFor(r => r.Take)
                .GreaterThan(0)
                .LessThanOrEqualTo(100);
        }
    }
}
