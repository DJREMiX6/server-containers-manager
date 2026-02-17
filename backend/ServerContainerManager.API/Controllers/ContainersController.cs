using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServerContainerManager.API.Models.Responses;
using ServerContainerManager.API.Models.Responses.Extensions;
using ServerContainerManager.API.Services;
using ServerContainerManager.API.Services.Abstraction;

namespace ServerContainerManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContainersController : ControllerBase
    {
        private readonly ILogger<ContainersController> logger;
        private readonly IDockerQueryService dockerClientService;

        public ContainersController(ILogger<ContainersController> logger, IDockerQueryService dockerClientService)
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
