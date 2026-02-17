using Docker.DotNet;
using Docker.DotNet.Models;
using ServerContainerManager.Application.Services.Abstraction;

namespace ServerContainerManager.Application.Services
{
    public class DockerQueryService(DockerClient dockerClient) : IDockerQueryService
    {
        private readonly DockerClient _dockerClient = dockerClient;

        public async Task<IEnumerable<ContainerListResponse>> GetContainers(CancellationToken cancellationToken = default)
        {
            return await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters() { All = true }, cancellationToken);
        }
    }
}
