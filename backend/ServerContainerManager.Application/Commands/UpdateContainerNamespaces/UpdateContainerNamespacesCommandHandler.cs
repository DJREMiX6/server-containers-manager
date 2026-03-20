using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Queries.GetContainerList;
using ServerContainerManager.Domain.Entities.Auth;
using ServerContainerManager.Domain.Entities.Containers;

namespace ServerContainerManager.Application.Commands.UpdateContainerNamespaces
{
    internal sealed class UpdateContainerNamespacesCommandHandler(
        ILogger<UpdateContainerNamespacesCommandHandler> logger,
        AppDbContext dbContext,
        UserManager<AppUser> userManager) : ICommandHandler<UpdateContainerNamespacesCommand, UpdateContainerNamespacesCommandResult>
    {
        private readonly ILogger<UpdateContainerNamespacesCommandHandler> _logger = logger;
        private readonly AppDbContext _appDbContext = dbContext;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<UpdateContainerNamespacesCommandResult>> HandleAsync(UpdateContainerNamespacesCommand command, CancellationToken cancellationToken = default)
        {
            var namespaces = await _appDbContext.Namespaces.Where(n => command.NamespacesIds.Contains(n.Id)).ToListAsync(cancellationToken);
            if (namespaces.Count != command.NamespacesIds.Count)
                return Error.Validation($"{nameof(UpdateContainerNamespacesCommandHandler)}.{nameof(HandleAsync)}", "Some namespaces do not exist");

            var container = await _appDbContext.Containers
                .Where(c => c.Id == command.ContainerId)
                .FirstOrDefaultAsync(cancellationToken);

            if (container == null)
                return Error.NotFound($"{nameof(UpdateContainerNamespacesCommandHandler)}.{nameof(HandleAsync)}", $"Cannot find container with id ${command.ContainerId}");

            container.UpdateNamespaces(namespaces);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return new UpdateContainerNamespacesCommandResult();
        }

        private async Task<ErrorOr<AppUser>> GetUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Where(u => u.Id == userId).Include(u => u.Namespaces).FirstOrDefaultAsync(cancellationToken);
            if (user == null)
                return Error.Unauthorized($"{nameof(GetContainerListQueryHandler)}.{nameof(GetUserAsync)}", $"Cannot find user {userId}");

            return user;
        }
    }
}
