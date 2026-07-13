using FluentValidation;
using ServerContainerManager.Application.Models;

namespace ServerContainerManager.API.Models.Requests.ContainersController
{
    public sealed record GetContainersRequest
    {
        public int? Skip { get; init; }
        public int? Take { get; init; }
        public ContainerSortBy? SortBy { get; init; }
        public SortOrder? Order { get; init; }
    }

    public sealed class GetContainersRequestValidator : AbstractValidator<GetContainersRequest>
    {
        public GetContainersRequestValidator()
        {
            RuleFor(r => r.Skip)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Invalid Skip parameter, cannot be less than zero.");
            RuleFor(r => r.Take)
                .GreaterThan(0)
                .WithMessage("Invalid Take parameter, cannot be less or equal to zero.")
                .LessThanOrEqualTo(100)
                .WithMessage("Invalid Take parameter, cannot be greater or equal to zero.");
        }
    }
}
