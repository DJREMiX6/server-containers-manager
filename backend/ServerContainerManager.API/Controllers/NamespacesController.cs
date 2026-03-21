using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServerContainerManager.API.Extensions;
using ServerContainerManager.API.Models.Requests.NamespacesController;
using ServerContainerManager.API.Models.Responses.Extensions;
using ServerContainerManager.API.Models.Responses.NamespacesController;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Commands.CreateNamespace;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Application.Queries.Abstraction;
using ServerContainerManager.Application.Queries.GetNamespacesList;

namespace ServerContainerManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NamespacesController(ILogger<ContainersController> logger) : ControllerBase
    {
        private readonly ILogger<ContainersController> _logger = logger;

        [HttpGet]
        public async Task<Results<Ok<GetNamespacesListResponse>, ProblemHttpResult>> GetNamespaces(
            [FromServices] IQueryHandler<GetNamespacesListQuery, GetNamespacesListQueryResult> queryHandler,
            CancellationToken cancellationToken = default)
        {
            var query = new GetNamespacesListQuery
            {
                UserId = User.GetUserId()
            };

            var result = await queryHandler.HandleAsync(query, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return TypedResults.Ok(result.Value.ToContract());
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<Results<Ok<CreateNamespaceResponse>, ProblemHttpResult>> CreateNamespace(
            [FromBody] CreateNamespaceRequest request,
            [FromServices] ICommandHandler<CreateNamespaceCommand, CreateNamespaceCommandResult> commandHandler,
            CancellationToken cancellationToken = default)
        {
            var command = new CreateNamespaceCommand() { Name = request.Name };
        
            var result = await commandHandler.HandleAsync(command, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return TypedResults.Ok(result.Value.ToContract());
        }
    }
}
