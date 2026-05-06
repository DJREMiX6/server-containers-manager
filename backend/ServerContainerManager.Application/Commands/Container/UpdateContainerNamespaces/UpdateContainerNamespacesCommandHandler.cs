using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Extensions;
using ServerContainerManager.Domain.Entities.Auth;
using ServerContainerManager.Shared.Utils;
using ServerContainerManager.Shared.Utils.Errors;
using ServerContainerManager.Shared.Utils.Extensions;

namespace ServerContainerManager.Application.Commands.Container.UpdateContainerNamespaces
{
    internal sealed class UpdateContainerNamespacesCommandHandler(
        ILogger<UpdateContainerNamespacesCommandHandler> logger,
        AppDbContext dbContext,
        UserManager<AppUser> userManager,
        TimeProvider timeProvider) : ICommandHandler<UpdateContainerNamespacesCommand, UpdateContainerNamespacesCommandResult>
    {
        private readonly ILogger<UpdateContainerNamespacesCommandHandler> _logger = logger;
        private readonly AppDbContext _appDbContext = dbContext;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly TimeProvider _timeProvider = timeProvider;

        public async Task<ErrorOr<UpdateContainerNamespacesCommandResult>> HandleAsync(UpdateContainerNamespacesCommand command, CancellationToken cancellationToken = default)
        {
            if (!await _userManager.UserExistsByIdAsync(command.UserId, cancellationToken))
                return UserErrors.UnauthorizedNotFound(command.UserId);

            var actor = Actor.FromUser(command.UserId);
            var now = _timeProvider.GetUtcDateTimeNow();

            var namespaces = await _appDbContext.Namespaces.Where(n => command.NamespacesIds.Contains(n.Id)).ToListAsync(cancellationToken);
            if (namespaces.Count != command.NamespacesIds.Count)
                return NamespaceErrors.SomeNotExist(command.NamespacesIds.Except(namespaces.Select(n => n.Id)));

            var container = await _appDbContext.Containers
                .Where(c => c.Id == command.ContainerId)
                .FirstOrDefaultAsync(cancellationToken);

            if (container == null)
                return ContainerErrors.NotFound(command.ContainerId);

            container.UpdateNamespaces(namespaces, actor, now);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return new UpdateContainerNamespacesCommandResult();
        }
    }
}
