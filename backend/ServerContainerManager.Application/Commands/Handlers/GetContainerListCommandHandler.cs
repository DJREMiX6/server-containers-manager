using Docker.DotNet;
using Docker.DotNet.Models;
using ServerContainerManager.Application.Commands.Abstraction;

namespace ServerContainerManager.Application.Commands.Handlers
{
    internal class GetContainerListCommandHandler(DockerClient dockerClient) : IGetContainerListCommandHandler
    {
        private readonly DockerClient _dockerClient = dockerClient;

        public async Task<IEnumerable<GetContainerListCommandResult>> HandleAsync(GetContainerListCommand command, CancellationToken cancellationToken = default)
        {
            var containers = await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters() { All = true }, cancellationToken);
            return ToCommandResult(containers);
        }

        private static IEnumerable<GetContainerListCommandResult> ToCommandResult(IEnumerable<ContainerListResponse> responses) =>
            responses.Select(r => new GetContainerListCommandResult()
            {
                Id = r.ID,
                Name = r.Names[0],
                Status = r.State,
                Created = r.Created,
                Labels = r.Labels,
                PrivatePorts = r.Ports.Select(p => p.PrivatePort),
                PublicPorts = r.Ports.Select(p => p.PublicPort)
            });
    }
}
