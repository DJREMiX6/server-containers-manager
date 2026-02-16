using Docker.DotNet;
using Docker.DotNet.Models;

namespace ServerContainerManager.API.Services
{
    public class DockerQueryService
    {
        private const string DockerSocketUriPath = "unix:///var/run/docker.sock";

        private readonly DockerClient _dockerClient;

        public DockerQueryService()
        {
            var dockerSocketUri = new Uri(DockerSocketUriPath);
            _dockerClient = new DockerClientConfiguration(dockerSocketUri).CreateClient();
        }

        public async Task<IEnumerable<ContainerListResponse>> GetContainers(CancellationToken cancellationToken = default)
        {
            return await _dockerClient.Containers.ListContainersAsync(new Docker.DotNet.Models.ContainersListParameters() { All = true }, cancellationToken);
        }
    }
}
