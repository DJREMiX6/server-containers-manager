using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServerContainerManager.API.Extensions;
using ServerContainerManager.API.Models.Requests.Auth;
using ServerContainerManager.API.Models.Requests.UsersController;
using ServerContainerManager.API.Models.Responses.Extensions;
using ServerContainerManager.API.Models.Responses.UsersController;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Commands.User.ChangePassword;
using ServerContainerManager.Application.Commands.User.CreateUser;
using ServerContainerManager.Application.Commands.User.DeleteUser;
using ServerContainerManager.Application.Commands.User.GetUserList;
using ServerContainerManager.Application.Commands.User.UpdateUserNamespaces;
using ServerContainerManager.Application.Consts;

namespace ServerContainerManager.API.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(ILogger<UsersController> logger) : Controller
    {
        private readonly ILogger<UsersController> _logger = logger;

        [HttpGet]
        public async Task<Ok<GetUserListResponse>> GetUserList(
            ICommandHandler<GetUserListCommand, GetUserListCommandResult> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new GetUserListCommand();
            var getUserListResult = await handler.HandleAsync(command, cancellationToken);

            return TypedResults.Ok(getUserListResult.Value.ToContract());
        }

        [HttpPost]
        public async Task<Results<Ok<Guid>, ProblemHttpResult>> CreateUser(
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

            return TypedResults.Ok(createUserResult.Value.UserId);
        }

        [HttpPatch("{userId:guid}/change-password")]
        public async Task<Results<Ok, ProblemHttpResult>> ChangeUserPassword(
            Guid userId,
            ChangePasswordRequest request,
            ICommandHandler<ChangePasswordCommand, ChangePasswordCommandResult> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new ChangePasswordCommand()
            {
                UserId = userId,
                CurrentPassword = request.CurrentPassword,
                NewPassword = request.NewPassword
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
