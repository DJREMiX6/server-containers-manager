using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Shared.Utils.Errors;

namespace ServerContainerManager.Application.Commands.Namespace.UpdateNamespaceAssociatedContainers
{
    internal class UpdateNamespaceAssociatedContainersCommandHandler(
        ILogger<UpdateNamespaceAssociatedContainersCommandHandler> logger,
        AppDbContext dbContext) : ICommandHandler<UpdateNamespaceAssociatedContainersCommand, UpdateNamespaceAssociatedContainersCommandResult>
    {
        private readonly ILogger<UpdateNamespaceAssociatedContainersCommandHandler> _logger = logger;
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<ErrorOr<UpdateNamespaceAssociatedContainersCommandResult>> HandleAsync(UpdateNamespaceAssociatedContainersCommand command, CancellationToken cancellationToken = default)
        {
            var @namespace = await _dbContext.Namespaces
                .Where(n => n.Id == command.NamespaceId)
                .Include(n => n.AssociatedContainers)
                .FirstOrDefaultAsync(cancellationToken);
            if (@namespace is null)
                return NamespaceErrors.NotFound(command.NamespaceId);

            var containers = await _dbContext.Containers
                .Where(c => command.AssociatedContainerIds.Contains(c.Id))
                .ToListAsync(cancellationToken);
            if (containers.Count != command.AssociatedContainerIds.Count)
                return ContainerErrors.NotFoundList([.. command.AssociatedContainerIds]);

            var result = @namespace.UpdateAssociatedContainers(containers);
            if (result.IsError) 
                return result.Errors;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateNamespaceAssociatedContainersCommandResult();
        }
    }
}
