using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServerContainerManager.API.Models.Responses;
using ServerContainerManager.API.Models.Responses.Extensions;
using ServerContainerManager.API.Services;

namespace ServerContainerManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContainersController : ControllerBase
    {
        private readonly ILogger<ContainersController> logger;
        private readonly DockerQueryService dockerClientService;

        public ContainersController(ILogger<ContainersController> logger, DockerQueryService dockerClientService)
        {
            this.logger = logger;
            this.dockerClientService = dockerClientService;
        }

        [HttpGet]
        public async Task<Ok<IEnumerable<GetContainerListResponse>>> GetAllContainers()
        {
            var containers = await dockerClientService.GetContainers();
            return TypedResults.Ok(containers.ToGetContainerListResponse());
        }
    }
}
