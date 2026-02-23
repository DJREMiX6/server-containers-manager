using Microsoft.AspNetCore.Identity;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Domain.Entities.Auth;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace ServerContainerManager.Application.Commands.SignIn
{
    internal class SignInCommandHandler(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager) : ICommandHandler<SignInCommand, SignInCommandResult>
    {
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<SignInCommandResult> HandleAsync(SignInCommand command, CancellationToken cancellationToken = default)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(
                command.Username,
                command.Password,
                command.IsPersistent,
                command.LockOutOnFailure);

            if (signInResult.IsNotAllowed)
                return new SignInCommandResult([Error.Forbidden($"{nameof(SignInCommandHandler)}.{nameof(HandleAsync)}", $"User {command.Username} is not allowed to sign in.")]);

            if(signInResult.IsLockedOut)
                return new SignInCommandResult([Error.Forbidden($"{nameof(SignInCommandHandler)}.{nameof(HandleAsync)}", $"User {command.Username} is locked out.")]);

            if (!signInResult.Succeeded)
                return new SignInCommandResult([Error.Unauthorized($"{nameof(SignInCommandHandler)}.{nameof(HandleAsync)}", $"Wrong credentials for user {command.Username}.")]);

            var user = (await _userManager.Users
                .Where(u => u.UserName == command.Username)
                .Include(u => u.Namespaces)
                .FirstOrDefaultAsync(cancellationToken))!;
            var roles = await _userManager.GetRolesAsync(user);

            return new SignInCommandResult(
                userId: user.Id,
                username: user.UserName!,
                roles: roles,
                user.Namespaces
                    .Select(n => new SignInCommandResultNamespace(Id: n.Id, Name: n.Name)));
        }
    }
}
