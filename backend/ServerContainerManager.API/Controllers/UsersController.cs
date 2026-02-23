using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServerContainerManager.API.Extensions;
using ServerContainerManager.API.Models.Requests;
using ServerContainerManager.API.Models.Responses;
using ServerContainerManager.API.Models.Responses.Extensions;
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
        public async Task<Ok<IEnumerable<GetUserListResponse>>> GetUserList(
            ICommandHandler<GetUserListCommand, IEnumerable<GetUserListCommandResult>> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new GetUserListCommand();
            var getUserListResult = await handler.HandleAsync(command, cancellationToken);

            return TypedResults.Ok(getUserListResult.ToContract());
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

            return TypedResults.Ok(createUserResult.UserId);
        }

        [HttpPatch("{userId:guid}/change-password")]
        public async Task<Results<Ok, ProblemHttpResult>> ChangePassword(
            Guid userId,
            ChangePasswordRequest request,
            ICommandHandler<ChangePasswordCommand, ChangePasswordCommandResult> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new ChangePasswordCommand(
                CallerUserId: User.GetUserId(),
                UserId: userId,
                CurrentPassword: request.CurrentPassword,
                NewPassword: request.NewPassword);

            var changePasswordResult = await handler.HandleAsync(command, cancellationToken);

            if(changePasswordResult.IsError)
                return changePasswordResult.Errors.ToProblemHttpResult();

            return TypedResults.Ok();
        }
    }
}
