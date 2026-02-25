using Microsoft.AspNetCore.Diagnostics;

namespace ServerContainerManager.API.ErrorHandling
{
    public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var errorId = Guid.NewGuid();

            _logger.LogError(exception, "Unhandled exception. ErrorId: {ErrorId}", errorId);

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(new { errorId }, cancellationToken);

            return true;
        }
    }
}
