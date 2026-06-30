using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ServerContainerManager.API.Extensions;
using ServerContainerManager.API.Models.Requests.NamespacesController;
using ServerContainerManager.API.Models.Responses.Extensions;
using ServerContainerManager.API.Models.Responses.NamespacesController;
using ServerContainerManager.API.Policies;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Commands.Namespace.CreateNamespace;
using ServerContainerManager.Application.Commands.Namespace.UpdateNamespaceAssociatedUsers;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Application.Queries.Abstraction;
using ServerContainerManager.Application.Queries.Namespace.CheckNamespaceNameAvailability;
using ServerContainerManager.Application.Queries.Namespace.GetNamespaceAssociatedUsers;
using ServerContainerManager.Application.Queries.Namespace.GetNamespacesList;

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
            var query = new GetNamespacesListQuery();

            var result = await queryHandler.HandleAsync(query, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return TypedResults.Ok(result.Value.ToContract());
        }

        [Authorize(Policy = AuthPolicies.ConfirmedAdminPolicy.Name)]
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

        [Authorize(Policy = AuthPolicies.ConfirmedAdminPolicy.Name)]
        [HttpGet("{namespaceId:guid}/users")]
        public async Task<Results<Ok<GetNamespaceUsersResponse>, ProblemHttpResult>> GetNamespaceUsers(
            [FromRoute] Guid namespaceId,
            [FromServices] IQueryHandler<GetNamespaceAssociatedUsersQuery, GetNamespaceAssociatedUsersQueryResult> queryHandler,
            CancellationToken cancellationToken = default)
        {
            var query = new GetNamespaceAssociatedUsersQuery()
            {
                NamespaceId = namespaceId,
            };

            var result = await queryHandler.HandleAsync(query, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return TypedResults.Ok(result.Value.ToContract());
        }

        [Authorize(Policy = AuthPolicies.ConfirmedAdminPolicy.Name)]
        [HttpPatch("{namespaceId:guid}/users")]
        public async Task<Results<NoContent, ProblemHttpResult>> UpdateNamespaceUsers(
            [FromRoute] Guid namespaceId,
            [FromBody] UpdateNamespaceUsersRequest updateNamespaceUsersRequest,
            [FromServices] ICommandHandler<UpdateNamespaceAssociatedUsersCommand, UpdateNamespaceAssociatedUsersCommandResult> commandHandler,
            CancellationToken cancellationToken = default)
        {
            var command = new UpdateNamespaceAssociatedUsersCommand()
            {
                NamespaceId = namespaceId,
                AssociatedUserIds = updateNamespaceUsersRequest.AssociatedUserIds
            };

            var result = await commandHandler.HandleAsync(command, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return TypedResults.NoContent();
        }

        [HttpHead("check-name")]
        public async Task<Results<NoContent, Conflict, ProblemHttpResult>> CheckNamespaceNameAvailability(
            [FromQuery] CheckNameAvailabilityRequest request,
            [FromServices] IQueryHandler<CheckNamespaceNameAvailabilityQuery, CheckNamespaceNameAvailabilityQueryResult> queryHandler,
            CancellationToken cancellationToken = default)
        {
            var query = new CheckNamespaceNameAvailabilityQuery()
            {
                Name = request.Name
            };

            var result = await queryHandler.HandleAsync(query, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return result.Value.IsAvailable
                ? TypedResults.NoContent()
                : TypedResults.Conflict();
        }
    }
}
