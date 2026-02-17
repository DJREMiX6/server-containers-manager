using FluentValidation;

namespace ServerContainerManager.API.Options.Validators
{
    public sealed class DockerOptionsValidator : AbstractValidator<DockerOptions>
    {
        public DockerOptionsValidator()
        {
            RuleFor(x => x.Endpoint)
                .NotEmpty()
                .WithMessage("Docker endpoint must not be empty.")
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Docker endpoint must be a valid URI.");
        }
    }
}
