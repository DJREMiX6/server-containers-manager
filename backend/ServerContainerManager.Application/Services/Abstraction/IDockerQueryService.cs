using Docker.DotNet.Models;

namespace ServerContainerManager.Application.Services.Abstraction
{
    public interface IDockerQueryService
    {
        public Task<IEnumerable<ContainerListResponse>> GetContainers(CancellationToken cancellationToken = default);
    }
}
