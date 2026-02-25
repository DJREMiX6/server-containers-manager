using Docker.DotNet;
using Docker.DotNet.Models;
using ErrorOr;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Entities;

namespace ServerContainerManager.Application.Commands.GetContainerList
{
    internal class GetContainerListCommandHandler(ILogger<GetContainerListCommandHandler> logger, AppDbContext appDbContext, DockerClient dockerClient) : ICommandHandler<GetContainerListCommand, GetContainerListCommandResult>
    {
        private readonly ILogger<GetContainerListCommandHandler> _logger = logger;
        private readonly AppDbContext _appDbContext = appDbContext;
        private readonly DockerClient _dockerClient = dockerClient;

        public async Task<ErrorOr<GetContainerListCommandResult>> HandleAsync(GetContainerListCommand command, CancellationToken cancellationToken = default)
        {
            using var transaction = await _appDbContext.Database.BeginTransactionAsync(cancellationToken);

            var containers = await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters() { All = true }, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            
            return new GetContainerListCommandResult(containers
                .Select(r => new GetContainerListCommandResultContainerInfo(
                    id: r.ID,
                    name: r.Names[0],
                    status: r.State,
                    created: r.Created,
                    labels: r.Labels,
                    privatePorts: [.. r.Ports.Select(p => p.PrivatePort)],
                    publicPorts: [.. r.Ports.Select(p => p.PublicPort)]))
                .ToList());
        }
    }
}
