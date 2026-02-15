using Docker.DotNet.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServerContainerManager.API.Services;

namespace ServerContainerManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContainersController : ControllerBase
    {
        private readonly ILogger<ContainersController> logger;
        private readonly DockerClientService dockerClientService;

        public ContainersController(ILogger<ContainersController> logger, DockerClientService dockerClientService)
        {
            this.logger = logger;
            this.dockerClientService = dockerClientService;
        }

        [HttpGet]
        public async Task<Ok<IEnumerable<ContainerListResponse>>> GetAllContainers()
        {
            var containers = await dockerClientService.GetContainers();
            return TypedResults.Ok(containers);
        }
    }
}
