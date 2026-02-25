using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerContainerManager.API.Extensions;
using ServerContainerManager.API.Models.Requests.Auth;
using ServerContainerManager.API.Models.Responses.AuthController;
using ServerContainerManager.API.Models.Responses.Extensions;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Commands.ChangePassword;
using ServerContainerManager.Application.Commands.CreateUser;
using ServerContainerManager.Application.Commands.GetSessionInfo;
using ServerContainerManager.Application.Commands.GetUserList;
using ServerContainerManager.Application.Commands.SignIn;
using ServerContainerManager.Application.Commands.SignOut;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(ILogger<AuthController> logger) : ControllerBase
    {
        private readonly ILogger<AuthController> _logger = logger;

        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<Results<Ok, ProblemHttpResult>> SignIn(
            SignInRequest request,
            ICommandHandler<SignInCommand, SignInCommandResult> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new SignInCommand(request.Username, request.Password, isPersistent: true, lockOutOnFailure: true);

            var signInResult = await handler.HandleAsync(command, cancellationToken);

            if (signInResult.IsError)
                return signInResult.Errors.ToProblemHttpResult();

            return TypedResults.Ok();
        }

        [AllowAnonymous]
        [HttpPost("signout")]
        public async Task<Results<Ok, ProblemHttpResult>> SignOut(
            ICommandHandler<SignOutCommand, SignOutCommandResult> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new SignOutCommand();
            var signOutResult = await handler.HandleAsync(command, cancellationToken);

            if (signOutResult.IsError)
                signOutResult.Errors.ToProblemHttpResult();

            return TypedResults.Ok();
        }

        [HttpGet("session")]
        public async Task<Results<Ok<GetSessionInfoResponse>, ProblemHttpResult>> GetSessionInfo(
            ICommandHandler<GetSessionInfoCommand, GetSessionInfoCommandResult> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new GetSessionInfoCommand(User.GetUserId());
            var getSessionInfoResult = await handler.HandleAsync(command, cancellationToken);

            if(getSessionInfoResult.IsError)
                return getSessionInfoResult.Errors.ToProblemHttpResult();

            return TypedResults.Ok(getSessionInfoResult.Value.ToContract());
        }

        [HttpPost("change-password")]
        public async Task<Results<NoContent, ProblemHttpResult>> ChangePassword(
            ChangePasswordRequest request,
            ICommandHandler<ChangePasswordCommand, ChangePasswordCommandResult> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new ChangePasswordCommand(User.GetUserId(), request.CurrentPassword, request.NewPassword);
            var result = await handler.HandleAsync(command, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return TypedResults.NoContent();
        }
    }
}
