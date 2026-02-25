using FluentValidation;

namespace ServerContainerManager.API.Models.Requests.Auth
{
    public sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);

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
