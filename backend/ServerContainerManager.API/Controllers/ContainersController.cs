using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServerContainerManager.API.Extensions;
using ServerContainerManager.API.Models.Requests.ContainersController;
using ServerContainerManager.API.Models.Responses.ContainersController;
using ServerContainerManager.API.Models.Responses.Extensions;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Commands.Container.KillContainer;
using ServerContainerManager.Application.Commands.Container.PauseContainer;
using ServerContainerManager.Application.Commands.Container.RestartContainer;
using ServerContainerManager.Application.Commands.Container.ResumeContainer;
using ServerContainerManager.Application.Commands.Container.StartContainer;
using ServerContainerManager.Application.Commands.Container.StopContainer;
using ServerContainerManager.Application.Commands.Container.UpdateContainerNamespaces;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Application.Queries.Abstraction;
using ServerContainerManager.Application.Queries.Container.GetContainerList;

namespace ServerContainerManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContainersController(ILogger<ContainersController> logger) : ControllerBase
    {
        private readonly ILogger<ContainersController> _logger = logger;

        [HttpGet]
        public async Task<Results<Ok<GetContainerListResponse>, ProblemHttpResult>> GetContainers(
            [FromQuery] GetContainersRequest request,
            [FromServices] IQueryHandler<GetContainerListQuery, GetContainerListQueryResult> queryHandler,
            CancellationToken cancellationToken = default)
        {
            var query = new GetContainerListQuery
            {
                UserId = User.GetUserId(),
                Skip = request.Skip,
                Take = request.Take,
                SortBy = request.SortBy,
                Order = request.Order
            };

            var result = await queryHandler.HandleAsync(query, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return TypedResults.Ok(result.Value.ToContract());
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPatch("{containerId}/namespaces")]
        public async Task<Results<NoContent, ProblemHttpResult>> UpdateContainerNamespaces(
            [FromRoute] string containerId,
            [FromBody] UpdateContainerNamespacesRequest request,
            [FromServices] ICommandHandler<UpdateContainerNamespacesCommand, UpdateContainerNamespacesCommandResult> commandHandler,
            CancellationToken cancellationToken = default)
        {
            var userId = User.GetUserId();
            var command = new UpdateContainerNamespacesCommand()
            {
                UserId = userId,
                ContainerId = containerId,
                NamespacesIds = request.NamespacesIds,
            };

            var result = await commandHandler.HandleAsync(command, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return TypedResults.NoContent();
        }

        [Authorize(Roles = UserRoles.Member)]
        [HttpPost("{containerId}/start")]
        public async Task<Results<NoContent, ProblemHttpResult>> StartContainer(
            [FromRoute] string containerId,
            [FromServices]  ICommandHandler<StartContainerCommand, StartContainerCommandResult> commandHandler,
            CancellationToken cancellationToken = default)
        {
            var userId = User.GetUserId();
            var command = new StartContainerCommand()
            {
                UserId = userId,
                ContainerId = containerId
            };

            var result = await commandHandler.HandleAsync(command, cancellationToken);

            if(result.IsError)
                return result.Errors.ToProblemHttpResult();

            return TypedResults.NoContent();
        }

        [Authorize(Roles = UserRoles.Member)]
        [HttpPost("{containerId}/stop")]
        public async Task<Results<NoContent, ProblemHttpResult>> StopContainer(
            [FromRoute] string containerId,
            [FromServices] ICommandHandler<StopContainerCommand, StopContainerCommandResult> commandHandler,
            CancellationToken cancellationToken = default)
        {
            var userId = User.GetUserId();
            var command = new StopContainerCommand()
            {
                UserId = userId,
                ContainerId = containerId
            };

            var result = await commandHandler.HandleAsync(command, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return TypedResults.NoContent();
        }

        [Authorize(Roles = UserRoles.Member)]
        [HttpPost("{containerId}/restart")]
        public async Task<Results<NoContent, ProblemHttpResult>> RestartContainer(
            [FromRoute] string containerId,
            [FromServices] ICommandHandler<RestartContainerCommand, RestartContainerCommandResult> commandHandler,
            CancellationToken cancellationToken = default)
        {
            var userId = User.GetUserId();
            var command = new RestartContainerCommand()
            {
                UserId = userId,
                ContainerId = containerId
            };

            var result = await commandHandler.HandleAsync(command, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return TypedResults.NoContent();
        }

        [Authorize(Roles = UserRoles.Member)]
        [HttpPost("{containerId}/pause")]
        public async Task<Results<NoContent, ProblemHttpResult>> PauseContainer(
            [FromRoute] string containerId,
            [FromServices] ICommandHandler<PauseContainerCommand, PauseContainerCommandResult> commandHandler,
            CancellationToken cancellationToken = default)
        {
            var userId = User.GetUserId();
            var command = new PauseContainerCommand()
            {
                UserId = userId,
                ContainerId = containerId
            };

            var result = await commandHandler.HandleAsync(command, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return TypedResults.NoContent();
        }

        [Authorize(Roles = UserRoles.Member)]
        [HttpPost("{containerId}/resume")]
        public async Task<Results<NoContent, ProblemHttpResult>> ResumeContainer(
            [FromRoute] string containerId,
            [FromServices] ICommandHandler<ResumeContainerCommand, ResumeContainerCommandResult> commandHandler,
            CancellationToken cancellationToken = default)
        {
            var userId = User.GetUserId();
            var command = new ResumeContainerCommand()
            {
                UserId = userId,
                ContainerId = containerId
            };

            var result = await commandHandler.HandleAsync(command, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return TypedResults.NoContent();
        }

        [Authorize(Roles = UserRoles.Member)]
        [HttpPost("{containerId}/kill")]
        public async Task<Results<NoContent, ProblemHttpResult>> KillContainer(
            [FromRoute] string containerId,
            [FromServices] ICommandHandler<KillContainerCommand, KillContainerCommandResult> commandHandler,
            CancellationToken cancellationToken = default)
        {
            var userId = User.GetUserId();
            var command = new KillContainerCommand()
            {
                UserId = userId,
                ContainerId = containerId
            };

            var result = await commandHandler.HandleAsync(command, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return TypedResults.NoContent();
        }
    }
}
