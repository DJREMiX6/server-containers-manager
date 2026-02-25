using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Commands.Models;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Commands.UpdateUserNamespaces
{
    internal class UpdateUserNamespacesCommandHandler(UserManager<AppUser> userManager, AppDbContext dbContext) : ICommandHandler<UpdateUserNamespacesCommand, UpdateUserNamespacesCommandResult>
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<ErrorOr<UpdateUserNamespacesCommandResult>> HandleAsync(UpdateUserNamespacesCommand command, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users
                .Where(u => u.Id == command.UserId)
                .Include(u => u.Namespaces)
                .FirstOrDefaultAsync(cancellationToken);
            if (user is null)
                return Error.NotFound($"{nameof(UpdateUserNamespacesCommandHandler)}.{nameof(HandleAsync)}", $"Cannot find user {command.UserId}");

            var namespaces = await _dbContext.Namespaces
                .Where(n => command.NamespacesIds.Contains(n.Id))
                .ToListAsync(cancellationToken);
            if (namespaces.Count != command.NamespacesIds.Count)
                return Error.Validation($"{nameof(UpdateUserNamespacesCommandHandler)}.{nameof(HandleAsync)}", "Some namespaces do not exist");

            var upsertResult = user.UpsertNamespaces(namespaces);
            if (upsertResult.IsError)
                return upsertResult.Errors;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateUserNamespacesCommandResult([.. namespaces.Select(n => new NamespaceInfo(n.Id, n.Name))]);
        }
    }
}
