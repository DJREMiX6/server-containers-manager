using Docker.DotNet;
using Docker.DotNet.Models;
using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Extensions;
using ServerContainerManager.Domain.Entities.Auth;
using ServerContainerManager.Shared.Utils.Errors;
using ServerContainerManager.Shared.Utils.Extensions;
using Actor = ServerContainerManager.Shared.Utils.Actor;

namespace ServerContainerManager.Application.Commands.Container.KillContainer
{
    internal class KillContainerCommandHandler(
        ILogger<KillContainerCommandHandler> logger,
        AppDbContext dbContext,
        UserManager<AppUser> userManager,
        DockerClient dockerClient,
        TimeProvider timeProvider) : IQueryHandler<KillContainerCommand, KillContainerCommandResult>
    {
        private readonly ILogger<KillContainerCommandHandler> _logger = logger;
        private readonly AppDbContext _dbContext = dbContext;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly DockerClient _dockerClient = dockerClient;
        private readonly TimeProvider _timeProvider = timeProvider;

        public async Task<ErrorOr<KillContainerCommandResult>> HandleAsync(KillContainerCommand command, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserWithNamespacesAsync(command.UserId, cancellationToken);
            if (user == null)
                return UserErrors.UnauthorizedNotFound(command.UserId);

            var isUserAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);
            var containerQuery = _dbContext.Containers.Where(c => c.Id == command.ContainerId);

            if (!isUserAdmin)
            {
                var namespacesIds = user.Namespaces.Select(n => n.Id).ToList();
                containerQuery = containerQuery.FilterByNamespaces(namespacesIds);
            }

            var container = await containerQuery.FirstOrDefaultAsync(cancellationToken);
            if (container == null)
                return ContainerErrors.NotFound(command.ContainerId);

            var now = _timeProvider.GetUtcDateTimeNow();
            var actor = Actor.FromUser(command.UserId);

            var containerKillResult = container.Kill(actor, now);
            if (containerKillResult.IsError)
                return containerKillResult.Errors;

            await _dockerClient.Containers.KillContainerAsync(command.ContainerId, new ContainerKillParameters(), cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new KillContainerCommandResult();
        }
    }
}
