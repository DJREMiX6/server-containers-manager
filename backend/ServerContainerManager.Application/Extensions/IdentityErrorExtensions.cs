using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace ServerContainerManager.Application.Extensions
{
    internal static class IdentityErrorExtensions
    {
        public static Error ToError(this IdentityError error) => Error.Validation(error.Code, error.Description);

        public static IEnumerable<Error> ToError(this IEnumerable<IdentityError> errors) => errors.Select(ToError); 
    }
}
