using Docker.DotNet;
using Docker.DotNet.Models;

namespace ServerContainerManager.API.Services
{
    public class DockerClientService
    {
        private readonly DockerClient _dockerClient;

        public DockerClientService()
        {
            var dockerSocketUri = new Uri("unix:///var/run/docker.sock");
            _dockerClient = new DockerClientConfiguration(dockerSocketUri).CreateClient();
        }

        public async Task<IEnumerable<ContainerListResponse>> GetContainers(CancellationToken cancellationToken = default)
        {
            return await _dockerClient.Containers.ListContainersAsync(new Docker.DotNet.Models.ContainersListParameters() { All = true }, cancellationToken);
        }
    }
}
