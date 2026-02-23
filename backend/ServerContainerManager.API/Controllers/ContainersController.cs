using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServerContainerManager.API.Models.Responses;
using ServerContainerManager.API.Models.Responses.Extensions;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Commands.GetContainerList;
using ServerContainerManager.Application.Consts;

namespace ServerContainerManager.API.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContainersController(ILogger<ContainersController> logger) : ControllerBase
    {
        private readonly ILogger<ContainersController> _logger;

        [HttpGet]
        [Authorize(Roles = UserRoles.Member)]
        public async Task<Ok<IEnumerable<GetContainerListResponse>>> GetAllContainers(
            [FromServices] ICommandHandler<GetContainerListCommand, IEnumerable<GetContainerListCommandResult>> commandHandler,
            CancellationToken cancellationToken = default)
        {
            var command = new GetContainerListCommand();
            var result = await commandHandler.HandleAsync(command, cancellationToken);

            return TypedResults.Ok(result.ToContract());
        }
    }
}
