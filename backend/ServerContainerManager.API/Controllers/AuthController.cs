using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerContainerManager.API.Extensions;
using ServerContainerManager.API.Models.Requests;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Commands.CreateUser;
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
        

        [HttpPost("signin")]
        public async Task<Results<Ok, ProblemHttpResult>> SignIn(
            SignInRequest request,
            ICommandHandler<SignInCommand, SignInCommandResult> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new SignInCommand(request.Username, request.Password, IsPersistent: true, LockOutOnFailure: true);

            var signInResult = await handler.HandleAsync(command, cancellationToken);

            if (signInResult.IsError)
                return signInResult.Errors.ToProblemHttpResult();

            return TypedResults.Ok();
        }

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

        [HttpPost("users")]
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
    }
}
