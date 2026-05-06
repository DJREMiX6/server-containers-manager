using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServerContainerManager.API.Extensions;
using ServerContainerManager.API.Models.Requests.UsersController;
using ServerContainerManager.API.Models.Responses.Extensions;
using ServerContainerManager.API.Models.Responses.UsersController;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Commands.User.CreateUser;
using ServerContainerManager.Application.Commands.User.DeleteUser;
using ServerContainerManager.Application.Commands.User.ResetPassword;
using ServerContainerManager.Application.Commands.User.UpdateUserNamespaces;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Application.Queries.Abstraction;
using ServerContainerManager.Application.Queries.User.CheckUsernameAvailability;
using ServerContainerManager.Application.Queries.User.GetUserList;

namespace ServerContainerManager.API.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(ILogger<UsersController> logger) : ControllerBase
    {
        private readonly ILogger<UsersController> _logger = logger;

        [HttpGet]
        public async Task<Ok<GetUserListResponse>> GetUserList(
            [FromQuery]GetUserListQueryParameters queryParameters,
            IQueryHandler<GetUserListQuery, GetUserListQueryResult> handler,
            CancellationToken cancellationToken = default)
        {
            var username = queryParameters.Username;
            var command = new GetUserListQuery();
            var getUserListResult = await handler.HandleAsync(command, cancellationToken);

            return TypedResults.Ok(getUserListResult.Value.ToContract());
        }

        [HttpPost]
        public async Task<Results<Ok<CreateUserResponse>, ProblemHttpResult>> CreateUser(
            CreateUserRequest request,
            ICommandHandler<CreateUserCommand, CreateUserCommandResult> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new CreateUserCommand()
            {
                Username = request.Username,
                Password = request.Password
            };

            var createUserResult = await handler.HandleAsync(command, cancellationToken);

            if (createUserResult.IsError)
                return createUserResult.Errors.ToProblemHttpResult();

            return TypedResults.Ok(createUserResult.Value.ToContract());
        }

        [HttpHead("check-username")]
        public async Task<Results<NoContent, Conflict, ProblemHttpResult>> CheckUsernameAvailability(
            [FromQuery]CheckUsernameAvailabilityRequest request,
            IQueryHandler<CheckUsernameAvailabilityQuery, CheckUsernameAvailabilityQueryResult> query,
            CancellationToken cancellationToken = default)
        {
            var command = new CheckUsernameAvailabilityQuery()
            {
                Username = request.Username
            };
            
            var result = await query.HandleAsync(command, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return result.Value.IsAvailable
                ? TypedResults.NoContent()
                : TypedResults.Conflict();
        }

        [HttpPost("{userId:guid}/reset-password")]
        public async Task<Results<Ok, ProblemHttpResult>> ResetUserPassword(
            Guid userId,
            ResetUserPasswordRequest request,
            ICommandHandler<ResetUserPasswordCommand, ResetUserPasswordCommandResult> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new ResetUserPasswordCommand()
            {
                UserId = userId,
                Password = request.Password
            };

            var changePasswordResult = await handler.HandleAsync(command, cancellationToken);

            if(changePasswordResult.IsError)
                return changePasswordResult.Errors.ToProblemHttpResult();

            return TypedResults.Ok();
        }

        [HttpDelete("{userId:guid}")]
        public async Task<Results<Ok, ProblemHttpResult>> DeleteUser(
            Guid userId,
            ICommandHandler<DeleteUserCommand, DeleteUserCommandResult> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new DeleteUserCommand()
            {
                UserId = userId
            };
            var result = await handler.HandleAsync(command, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return TypedResults.Ok();
        }

        [HttpPatch("{userId:guid}/namespaces")]
        public async Task<Results<Ok, ProblemHttpResult>> UpdateUserNamespaces(
            Guid userId,
            UpdateUserNamespacesRequest request,
            ICommandHandler<UpdateUserNamespacesCommand, UpdateUserNamespacesCommandResult> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new UpdateUserNamespacesCommand
            {
                UserId = userId,
                NamespacesIds = request.NamespacesIds
            };
            var result = await handler.HandleAsync(command, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return TypedResults.Ok();
        }
    }
}
