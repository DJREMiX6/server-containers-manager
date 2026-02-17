using FluentValidation;
using Microsoft.Extensions.Options;
using ServerContainerManager.API.Options.Validators;

namespace ServerContainerManager.API.Extensions
{
    public static class OptionsBuilderExtensions
    {
        public static OptionsBuilder<TOptions> ValidateWithFluentValidator<TOptions>(this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
        {
            optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(sp =>
                new FluentValidateOptions<TOptions>(sp, optionsBuilder.Name));

            return optionsBuilder;
        }
    }
}
