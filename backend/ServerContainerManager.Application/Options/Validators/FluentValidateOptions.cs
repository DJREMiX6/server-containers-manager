using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ServerContainerManager.Application.Options.Validators
{
    public class FluentValidateOptions<TOptions>(IServiceProvider serviceProvider, string name) : IValidateOptions<TOptions> where TOptions : class
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly string _name = name;

        public ValidateOptionsResult Validate(string? name, TOptions options)
        {
            if (name is not null && name != _name)
                return ValidateOptionsResult.Skip;

            ArgumentNullException.ThrowIfNull(options);

            using var scope = _serviceProvider.CreateScope();

            var validator = scope.ServiceProvider.GetRequiredService<IValidator<TOptions>>();

            var validationResult = validator.Validate(options);

            if(validationResult.IsValid)
                return ValidateOptionsResult.Success;

            var type = options.GetType().Name;
            var errors = new List<string>();

            foreach (var failure in validationResult.Errors)
                errors.Add($"Validation failed for {type}.{failure.PropertyName} with the error: {failure.ErrorMessage}");
            
            return ValidateOptionsResult.Fail(errors);
        }
    }
}
