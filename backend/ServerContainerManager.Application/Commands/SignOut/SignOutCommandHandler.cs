using ErrorOr;
using Microsoft.AspNetCore.Identity;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Commands.SignOut
{
    internal class SignOutCommandHandler(SignInManager<AppUser> signInManager) : ICommandHandler<SignOutCommand, SignOutCommandResult>
    {
        private readonly SignInManager<AppUser> _signInManager = signInManager;

        public async Task<ErrorOr<SignOutCommandResult>> HandleAsync(SignOutCommand command, CancellationToken cancellationToken = default)
        {
            await _signInManager.SignOutAsync();

            return new SignOutCommandResult();
        }
    }
}
