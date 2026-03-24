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
using ServerContainerManager.Application.Queries.GetContainerList;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Commands.StopContainer
{
    internal class StopContainerCommandHandler(
        ILogger<StopContainerCommandHandler> logger,
        AppDbContext dbContext,
        UserManager<AppUser> userManager,
        DockerClient dockerClient) : ICommandHandler<StopContainerCommand, StopContainerCommandResult>
    {
        private readonly ILogger<StopContainerCommandHandler> _logger = logger;
        private readonly AppDbContext _dbContext = dbContext;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly DockerClient _dockerClient = dockerClient;

        public async Task<ErrorOr<StopContainerCommandResult>> HandleAsync(StopContainerCommand command, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users.GetUserWithNamespacesAsync(command.UserId, cancellationToken);
            if(user == null)
                return Error.Unauthorized($"{nameof(GetContainerListQueryHandler)}.{nameof(HandleAsync)}", $"Cannot find user {command.UserId}");
            var isUserAdmin = await _userManager.IsInRoleAsync(user, UserRoles.Admin);
            var containerQuery = _dbContext.Containers.Where(c => c.Id == command.ContainerId);
            
            if(!isUserAdmin)
            {
                var namespacesIds = user.Namespaces.Select(n => n.Id).ToList();
                containerQuery = containerQuery.FilterByNamespaces(namespacesIds);
            }

            if (!await containerQuery.AnyAsync(cancellationToken))
                return Error.NotFound($"{nameof(StopContainerCommandHandler)}.{nameof(HandleAsync)}", $"Cannot find container with id {command.ContainerId}");

            await _dockerClient.Containers.StopContainerAsync(command.ContainerId, new ContainerStopParameters(), cancellationToken);

            return new StopContainerCommandResult();
        }
    }
}
