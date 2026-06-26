using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Shared.Utils.Errors;

namespace ServerContainerManager.Application.Commands.Namespace.UpdateNamespaceAssociatedUsers
{
    internal sealed class UpdateNamespaceAssociatedUsersCommandHandler(
        ILogger<UpdateNamespaceAssociatedUsersCommandHandler> logger,
        AppDbContext dbContext) : ICommandHandler<UpdateNamespaceAssociatedUsersCommand, UpdateNamespaceAssociatedUsersCommandResult>
    {
        private readonly ILogger<UpdateNamespaceAssociatedUsersCommandHandler> _logger = logger;
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<ErrorOr<UpdateNamespaceAssociatedUsersCommandResult>> HandleAsync(UpdateNamespaceAssociatedUsersCommand command, CancellationToken cancellationToken = default)
        {
            var @namespace = await _dbContext.Namespaces.Include(n => n.AssociatedUsers).FirstOrDefaultAsync(n => n.Id == command.NamespaceId, cancellationToken);
            if(@namespace == null)
                return NamespaceErrors.NotFound(command.NamespaceId);

            var users = await _dbContext.Users.Where(u => command.AssociatedUserIds.Contains(u.Id)).ToListAsync(cancellationToken);
            if (users.Count != command.AssociatedUserIds.Count)
                return UserErrors.NotFoundList([.. command.AssociatedUserIds.Where(auId => !users.Any(u => u.Id == auId))]);

            var updateAssociatedUsersResult = @namespace.UpdateAssociatedUsers(users);
            if(updateAssociatedUsersResult.IsError)
                return updateAssociatedUsersResult.Errors;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateNamespaceAssociatedUsersCommandResult();
        }
    }
}
