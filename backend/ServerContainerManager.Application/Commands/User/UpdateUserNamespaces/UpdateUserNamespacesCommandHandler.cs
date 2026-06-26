using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Extensions;
using ServerContainerManager.Application.Models;
using ServerContainerManager.Domain.Entities.Auth;
using ServerContainerManager.Shared.Utils.Errors;

namespace ServerContainerManager.Application.Commands.User.UpdateUserNamespaces
{
    internal class UpdateUserNamespacesCommandHandler(ILogger<UpdateUserNamespacesCommandHandler> logger, AppDbContext dbContext, UserManager<AppUser> userManager) : ICommandHandler<UpdateUserNamespacesCommand, UpdateUserNamespacesCommandResult>
    {
        private readonly ILogger<UpdateUserNamespacesCommandHandler> _logger = logger;
        private readonly AppDbContext _dbContext = dbContext;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<UpdateUserNamespacesCommandResult>> HandleAsync(UpdateUserNamespacesCommand command, CancellationToken cancellationToken = default)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            var user = await _userManager.GetUserWithNamespacesAsync(command.UserId, cancellationToken);
            if (user is null)
                return UserErrors.UnauthorizedNotFound(command.UserId);

            var namespaces = await _dbContext.Namespaces
                .Where(n => command.NamespacesIds.Contains(n.Id))
                .ToListAsync(cancellationToken);
            if (namespaces.Count != command.NamespacesIds.Count)
                return NamespaceErrors.SomeNotExist(command.NamespacesIds.Except(namespaces.Select(n => n.Id)));

            var upsertResult = user.UpdateNamespaces(namespaces);
            if (upsertResult.IsError)
                return upsertResult.Errors;

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new UpdateUserNamespacesCommandResult()
            {
                Namespaces = [..namespaces.Select(NamespaceInfo.FromDomain)]
            };
        }
    }
}
