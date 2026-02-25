using Docker.DotNet;
using Docker.DotNet.Models;
using ErrorOr;
using ServerContainerManager.Application.Commands.Abstraction;

namespace ServerContainerManager.Application.Commands.GetContainerList
{
    internal class GetContainerListCommandHandler(DockerClient dockerClient) : ICommandHandler<GetContainerListCommand, GetContainerListCommandResult>
    {
        private readonly DockerClient _dockerClient = dockerClient;

        public async Task<ErrorOr<GetContainerListCommandResult>> HandleAsync(GetContainerListCommand command, CancellationToken cancellationToken = default)
        {
            var containers = await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters() { All = true }, cancellationToken);
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
