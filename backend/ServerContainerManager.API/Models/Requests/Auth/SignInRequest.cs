using FluentValidation;

namespace ServerContainerManager.API.Models.Requests.Auth
{
    public record SignInRequest(string Username, string Password);

    public class SignInRequestValidator : AbstractValidator<SignInRequest>
    {
        public SignInRequestValidator()
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
