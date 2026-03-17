using FluentValidation;

namespace ServerContainerManager.Application.Options.Validators
{
    internal class DockerContainersReconciliationOptionsValidator : AbstractValidator<DockerContainersReconciliationOptions>
    {
        public DockerContainersReconciliationOptionsValidator()
        {
            RuleFor(x => x.DockerConnectionMaxRetries)
                .GreaterThanOrEqualTo(0u).WithMessage($"{nameof(DockerContainersReconciliationOptions.DockerConnectionMaxRetries)} must be zero or greater");

            RuleFor(x => x.DockerConnectionRetryDelayMs)
                .GreaterThanOrEqualTo(0u).WithMessage($"{nameof(DockerContainersReconciliationOptions.DockerConnectionRetryDelayMs)} must be zero or greater");

            RuleFor(x => x.EventsSignalsProcessingDelayMs)
                .GreaterThanOrEqualTo(0u).WithMessage($"{nameof(DockerContainersReconciliationOptions.EventsSignalsProcessingDelayMs)} must be zero or greater");

            RuleFor(x => x.PeriodicReconciliationDelayMs)
                .GreaterThanOrEqualTo(0u).WithMessage($"{nameof(DockerContainersReconciliationOptions.PeriodicReconciliationDelayMs)} must be zero or greater");
        }
    }
}
