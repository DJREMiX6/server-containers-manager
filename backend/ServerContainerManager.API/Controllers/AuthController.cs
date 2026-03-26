using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServerContainerManager.API.Extensions;
using ServerContainerManager.API.Models.Requests.Auth;
using ServerContainerManager.API.Models.Requests.AuthController;
using ServerContainerManager.API.Models.Responses.AuthController;
using ServerContainerManager.API.Models.Responses.Extensions;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Commands.Auth.GetSessionInfo;
using ServerContainerManager.Application.Commands.Auth.SignIn;
using ServerContainerManager.Application.Commands.Auth.SignOut;
using ServerContainerManager.Application.Commands.User.ChangePassword;
using ServerContainerManager.Application.Commands.User.ChangeUsername;

namespace ServerContainerManager.API.Controllers
{
    [Authorize]
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
            var command = new SignInCommand()
            {
                Username = request.Username, 
                Password = request.Password, 
                IsPersistent = true, 
                LockOutOnFailure = true
            };

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
            var command = new GetSessionInfoCommand()
            {
                UserId = User.GetUserId()
            };
            var getSessionInfoResult = await handler.HandleAsync(command, cancellationToken);

            if(getSessionInfoResult.IsError)
                return getSessionInfoResult.Errors.ToProblemHttpResult();

            return TypedResults.Ok(getSessionInfoResult.Value.ToContract());
        }

        [HttpPost("user/change-password")]
        public async Task<Results<Ok, ProblemHttpResult>> ChangePassword(
            ChangePasswordRequest request,
            ICommandHandler<ChangePasswordCommand, ChangePasswordCommandResult> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new ChangePasswordCommand()
            {
                UserId = User.GetUserId(), 
                CurrentPassword = request.CurrentPassword, 
                NewPassword = request.NewPassword
            };
            var result = await handler.HandleAsync(command, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return TypedResults.Ok();
        }

        [HttpPatch("user")]
        public async Task<Results<Ok, ProblemHttpResult>> ChangeUsername(
            ChangeUsernameRequest request,
            ICommandHandler<ChangeUsernameCommand, ChangeUsernameCommandResult> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new ChangeUsernameCommand()
            {
                UserId = User.GetUserId(),
                NewUsername = request.NewUsername
            };
            var result = await handler.HandleAsync(command, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return TypedResults.Ok();
        }

    }
}
