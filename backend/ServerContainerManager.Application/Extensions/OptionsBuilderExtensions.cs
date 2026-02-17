using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ServerContainerManager.Application.Options.Validators;

namespace ServerContainerManager.Application.Extensions
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
