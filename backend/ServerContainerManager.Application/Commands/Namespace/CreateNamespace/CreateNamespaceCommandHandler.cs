using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Shared.Utils.Errors;

namespace ServerContainerManager.Application.Commands.Namespace.CreateNamespace
{
    internal class CreateNamespaceCommandHandler(
        ILogger<CreateNamespaceCommandHandler> logger,
        AppDbContext dbContext) : IQueryHandler<CreateNamespaceCommand, CreateNamespaceCommandResult>
    {
        private readonly ILogger<CreateNamespaceCommandHandler> _logger = logger;
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<ErrorOr<CreateNamespaceCommandResult>> HandleAsync(CreateNamespaceCommand command, CancellationToken cancellationToken = default)
        {
            var normalizedNamespaceName = command.Name.Trim();

            var namespaceExists = await _dbContext.Namespaces.AnyAsync(n => n.Name == normalizedNamespaceName, cancellationToken);
            if (namespaceExists)
                return NamespaceErrors.AlreadyExists(normalizedNamespaceName);

            var createNamespaceResult = Domain.Entities.Namespaces.Namespace.Create(normalizedNamespaceName);
            if (createNamespaceResult.IsError)
                return createNamespaceResult.Errors;

            await _dbContext.Namespaces.AddAsync(createNamespaceResult.Value, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreateNamespaceCommandResult() { NamespaceId = createNamespaceResult.Value.Id };
        }
    }
}
