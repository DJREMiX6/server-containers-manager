using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServerContainerManager.API.Models.Responses;
using ServerContainerManager.API.Models.Responses.Extensions;
using ServerContainerManager.Application.Commands;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Consts;

namespace ServerContainerManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContainersController : ControllerBase
    {
        private readonly ILogger<ContainersController> logger;

        public ContainersController(ILogger<ContainersController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Member)]
        public async Task<Ok<IEnumerable<GetContainerListResponse>>> GetAllContainers(
            [FromServices] IGetContainerListCommandHandler commandHandler,
            CancellationToken cancellationToken = default)
        {
            var command = new GetContainerListCommand();
            var result = await commandHandler.HandleAsync(command, cancellationToken);

            return TypedResults.Ok(result.ToContract());
        }
    }
}
