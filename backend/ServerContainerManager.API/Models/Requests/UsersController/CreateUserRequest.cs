using FluentValidation;

namespace ServerContainerManager.API.Models.Requests.UsersController
{
    public record CreateUserRequest(string Username, string Password);

    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username field cannot be empty");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password field cannot be empty");
        }
    }
}
