using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Commands.Auth.SignOut
{
    internal class SignOutCommandHandler(ILogger<SignOutCommandHandler> logger, SignInManager<AppUser> signInManager) : IQueryHandler<SignOutCommand, SignOutCommandResult>
    {
        private readonly ILogger<SignOutCommandHandler> logger = logger;
        private readonly SignInManager<AppUser> _signInManager = signInManager;

        public async Task<ErrorOr<SignOutCommandResult>> HandleAsync(SignOutCommand command, CancellationToken cancellationToken = default)
        {
            await _signInManager.SignOutAsync();

            return new SignOutCommandResult();
        }
    }
}
