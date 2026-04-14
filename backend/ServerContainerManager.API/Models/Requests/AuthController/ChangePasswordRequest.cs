using FluentValidation;

namespace ServerContainerManager.API.Models.Requests.Auth
{
    public sealed record ChangePasswordRequest
    {
        public required string CurrentPassword { get; init; }
        public required string NewPassword { get; init; }
    }

    public sealed class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithMessage("Current Password field cannot be empty");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("New Password field cannot be empty");
        }
    }
}
