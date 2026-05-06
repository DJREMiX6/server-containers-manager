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

namespace ServerContainerManager.Application.Commands.Container.StopContainer
{
    internal class StopContainerCommandHandler(
        ILogger<StopContainerCommandHandler> logger,
        AppDbContext dbContext,
        UserManager<AppUser> userManager,
        DockerClient dockerClient,
        TimeProvider timeProvider) : ICommandHandler<StopContainerCommand, StopContainerCommandResult>
    {
        private readonly ILogger<StopContainerCommandHandler> _logger = logger;
        private readonly AppDbContext _dbContext = dbContext;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly DockerClient _dockerClient = dockerClient;
        private readonly TimeProvider _timeProvider = timeProvider;

        public async Task<ErrorOr<StopContainerCommandResult>> HandleAsync(StopContainerCommand command, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserWithNamespacesAsync(command.UserId, cancellationToken);
            if (user == null)
                return UserErrors.UnauthorizedNotFound(command.UserId);

            var isUserAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);
            var containerQuery = _dbContext.Containers.Where(c => c.Id == command.ContainerId);
            
            if(!isUserAdmin)
            {
                var namespacesIds = user.Namespaces.Select(n => n.Id).ToList();
                containerQuery = containerQuery.FilterByNamespaces(namespacesIds);
            }

            var container = await containerQuery.FirstOrDefaultAsync(cancellationToken);
            if (container == null)
                return ContainerErrors.NotFound(command.ContainerId);

            var actor = Actor.FromUser(command.UserId);
            var now = _timeProvider.GetUtcDateTimeNow();
            var stopResult = container.Stop(actor, now);

            if (stopResult.IsError)
                return stopResult.Errors;

            var containerStopped = await _dockerClient.Containers.StopContainerAsync(command.ContainerId, new ContainerStopParameters(), cancellationToken);
            if (!containerStopped)
                return Error.Unexpected($"{nameof(StopContainerCommandHandler)}.{nameof(HandleAsync)}", $"Could not start container {container.Id}");

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new StopContainerCommandResult();
        }
    }
}
