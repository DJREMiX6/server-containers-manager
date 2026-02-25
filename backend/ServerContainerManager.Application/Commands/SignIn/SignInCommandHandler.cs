using Microsoft.AspNetCore.Identity;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Domain.Entities.Auth;
using ErrorOr;
using Microsoft.Extensions.Logging;

namespace ServerContainerManager.Application.Commands.SignIn
{
    internal class SignInCommandHandler(ILogger<SignInCommandHandler> logger, SignInManager<AppUser> signInManager) : ICommandHandler<SignInCommand, SignInCommandResult>
    {
        private readonly ILogger<SignInCommandHandler> _logger = logger;
        private readonly SignInManager<AppUser> _signInManager = signInManager;

        public async Task<ErrorOr<SignInCommandResult>> HandleAsync(SignInCommand command, CancellationToken cancellationToken = default)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(
                command.Username,
                command.Password,
                command.IsPersistent,
                command.LockOutOnFailure);

            if (signInResult.IsNotAllowed)
                return Error.Forbidden($"{nameof(SignInCommandHandler)}.{nameof(HandleAsync)}", $"User {command.Username} is not allowed to sign in.");

            if(signInResult.IsLockedOut)
                return Error.Forbidden($"{nameof(SignInCommandHandler)}.{nameof(HandleAsync)}", $"User {command.Username} is locked out.");

            if (!signInResult.Succeeded)
                return Error.Unauthorized($"{nameof(SignInCommandHandler)}.{nameof(HandleAsync)}", $"Wrong credentials for user {command.Username}.");

            return new SignInCommandResult();
        }
    }
}
