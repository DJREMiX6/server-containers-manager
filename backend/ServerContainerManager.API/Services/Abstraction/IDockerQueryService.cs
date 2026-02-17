using Docker.DotNet.Models;

namespace ServerContainerManager.API.Services.Abstraction
{
    public interface IDockerQueryService
    {
        public Task<IEnumerable<ContainerListResponse>> GetContainers(CancellationToken cancellationToken = default);
    }
}
