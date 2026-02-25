using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServerContainerManager.API.Extensions;
using ServerContainerManager.API.Models.Requests.Auth;
using ServerContainerManager.API.Models.Responses.Extensions;
using ServerContainerManager.API.Models.Responses.UsersController;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Commands.ChangePassword;
using ServerContainerManager.Application.Commands.CreateUser;
using ServerContainerManager.Application.Commands.GetUserList;
using ServerContainerManager.Application.Consts;

namespace ServerContainerManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(ILogger<UsersController> logger) : Controller
    {
        private readonly ILogger<UsersController> _logger = logger;

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Ok<GetUserListResponse>> GetUserList(
            ICommandHandler<GetUserListCommand, GetUserListCommandResult> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new GetUserListCommand();
            var getUserListResult = await handler.HandleAsync(command, cancellationToken);

            return TypedResults.Ok(getUserListResult.Value.ToContract());
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Results<Ok<Guid>, ProblemHttpResult>> CreateUser(
            CreateUserRequest request,
            ICommandHandler<CreateUserCommand, CreateUserCommandResult> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new CreateUserCommand(request.Username, request.Password);

            var createUserResult = await handler.HandleAsync(command, cancellationToken);

            if (createUserResult.IsError)
                return createUserResult.Errors.ToProblemHttpResult();

            return TypedResults.Ok(createUserResult.Value.UserId);
        }

        [HttpPatch("{userId:guid}/change-password")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<Results<Ok, ProblemHttpResult>> ChangeUserPassword(
            Guid userId,
            ChangePasswordRequest request,
            ICommandHandler<ChangePasswordCommand, ChangePasswordCommandResult> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new ChangePasswordCommand(
                userId,
                request.CurrentPassword,
                request.NewPassword);

            var changePasswordResult = await handler.HandleAsync(command, cancellationToken);

            if(changePasswordResult.IsError)
                return changePasswordResult.Errors.ToProblemHttpResult();

            return TypedResults.Ok();
        }
    }
}
