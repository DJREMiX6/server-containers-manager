using Microsoft.AspNetCore.Identity;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Domain.Entities.Auth;
using ErrorOr;

namespace ServerContainerManager.Application.Commands.SignIn
{
    internal class SignInCommandHandler(SignInManager<AppUser> signInManager) : ICommandHandler<SignInCommand, SignInCommandResult>
    {
        private readonly SignInManager<AppUser> _signInManager = signInManager;

        public async Task<SignInCommandResult> HandleAsync(SignInCommand command, CancellationToken cancellationToken = default)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(
                command.Username,
                command.Password,
                command.IsPersistent,
                command.LockOutOnFailure);

            if (signInResult.IsNotAllowed)
                return new SignInCommandResult(IsError: true, [Error.Forbidden($"{nameof(SignInCommandHandler)}.{nameof(HandleAsync)}", $"User {command.Username} is not allowed to sign in.")]);

            if(signInResult.IsLockedOut)
                return new SignInCommandResult(IsError: true, [Error.Forbidden($"{nameof(SignInCommandHandler)}.{nameof(HandleAsync)}", $"User {command.Username} is locked out.")]);

            if (!signInResult.Succeeded)
                return new SignInCommandResult(IsError: true, [Error.Unauthorized($"{nameof(SignInCommandHandler)}.{nameof(HandleAsync)}", $"Wrong credentials for user {command.Username}.")]);

            return new SignInCommandResult(IsError: false, []);
        }
    }
}
