using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Queries.Abstraction;
using ServerContainerManager.Shared.Utils.Errors;

namespace ServerContainerManager.Application.Queries.Namespace.CheckNamespaceNameAvailability
{
    internal class CheckNamespaceNameAvailabilityQueryHandler(
        ILogger<CheckNamespaceNameAvailabilityQueryHandler> logger,
        AppDbContext dbContext) : IQueryHandler<CheckNamespaceNameAvailabilityQuery, CheckNamespaceNameAvailabilityQueryResult>
    {
        private readonly ILogger<CheckNamespaceNameAvailabilityQueryHandler> _logger = logger;
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<ErrorOr<CheckNamespaceNameAvailabilityQueryResult>> HandleAsync(CheckNamespaceNameAvailabilityQuery query, CancellationToken cancellationToken = default)
        {
            var normalizedName = query.Name.Trim();
            var exists = await _dbContext.Namespaces.AnyAsync(n => n.Name == normalizedName, cancellationToken);

            return new CheckNamespaceNameAvailabilityQueryResult() { IsAvailable = !exists };
        }
    }
}
