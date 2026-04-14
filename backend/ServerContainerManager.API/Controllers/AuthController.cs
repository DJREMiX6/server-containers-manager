using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServerContainerManager.API.Extensions;
using ServerContainerManager.API.Models.Requests.Auth;
using ServerContainerManager.API.Models.Requests.AuthController;
using ServerContainerManager.API.Models.Responses.AuthController;
using ServerContainerManager.API.Models.Responses.Extensions;
using ServerContainerManager.API.Policies;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Commands.Auth.GetSessionInfo;
using ServerContainerManager.Application.Commands.Auth.SignIn;
using ServerContainerManager.Application.Commands.Auth.SignOut;
using ServerContainerManager.Application.Commands.User.ChangePassword;
using ServerContainerManager.Application.Commands.User.ChangeUsername;

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
            IQueryHandler<SignInCommand, SignInCommandResult> handler,
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
            IQueryHandler<SignOutCommand, SignOutCommandResult> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new SignOutCommand();
            var signOutResult = await handler.HandleAsync(command, cancellationToken);

            if (signOutResult.IsError)
                signOutResult.Errors.ToProblemHttpResult();

            return TypedResults.Ok();
        }

        [Authorize(Policy = AuthPolicies.AuthenticatedUserPolicy.Name)]
        [HttpGet("session")]
        public async Task<Results<Ok<GetSessionInfoResponse>, ProblemHttpResult>> GetSessionInfo(
            IQueryHandler<GetSessionInfoCommand, GetSessionInfoCommandResult> handler,
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

        [Authorize(Policy = AuthPolicies.AuthenticatedUserPolicy.Name)]
        [HttpPost("user/change-password")]
        public async Task<Results<Ok, ProblemHttpResult>> ChangePassword(
            ChangePasswordRequest request,
            IQueryHandler<ChangePasswordCommand, ChangePasswordCommandResult> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new ChangePasswordCommand()
            {
                UserId = User.GetUserId(), 
                CurrentPassword = request.CurrentPassword, 
                NewPassword = request.NewPassword,
            };
            var result = await handler.HandleAsync(command, cancellationToken);

            if (result.IsError)
                return result.Errors.ToProblemHttpResult();

            return TypedResults.Ok();
        }

        [Authorize]
        [HttpPatch("user")]
        public async Task<Results<Ok, ProblemHttpResult>> ChangeUsername(
            ChangeUsernameRequest request,
            IQueryHandler<ChangeUsernameCommand, ChangeUsernameCommandResult> handler,
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
