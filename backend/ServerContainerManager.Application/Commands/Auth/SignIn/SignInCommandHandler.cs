using Microsoft.AspNetCore.Identity;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Domain.Entities.Auth;
using ErrorOr;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Shared.Utils.Errors;

namespace ServerContainerManager.Application.Commands.Auth.SignIn
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
                return UserErrors.SignInNotAllowed(command.Username);

            if(signInResult.IsLockedOut)
                return UserErrors.LockedOut(command.Username);

            if (!signInResult.Succeeded)
                return UserErrors.InvalidCredentials(command.Username);

            return new SignInCommandResult();
        }
    }
}
