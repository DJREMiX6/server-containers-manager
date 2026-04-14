using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ServerContainerManager.API.Extensions
{
    public static class ErrorExtensions
    {
        public static ProblemHttpResult ToProblemHttpResult(this List<Error> errors)
        {
            if (errors.Count == 0)
                return TypedResults.Problem();

            if (errors.All(error => error.Type == ErrorType.Validation))
                return errors.ToValidationProblemHttpResult();

            return errors[0].ToProblemHttpResult();
        }

        public static ProblemHttpResult ToProblemHttpResult(this Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            // Remove description for possible sensitive results
            var description =
                statusCode == StatusCodes.Status401Unauthorized
                || statusCode == StatusCodes.Status403Forbidden
                ? ""
                : error.Description;

            return TypedResults.Problem(statusCode: statusCode, title: description);
        }

        private static ProblemHttpResult ToValidationProblemHttpResult(this List<Error> errors)
        {
            var validationErrors = errors
                .GroupBy(error => error.Code)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(error => error.Description).ToArray());

            return TypedResults.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "One or more validation errors occurred.",
                extensions: new Dictionary<string, object?> { ["errors"] = validationErrors });
        }
    }
}
