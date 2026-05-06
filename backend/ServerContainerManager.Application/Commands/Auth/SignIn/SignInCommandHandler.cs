using Microsoft.AspNetCore.Identity;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Domain.Entities.Auth;
using ErrorOr;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Shared.Utils.Errors;
using ServerContainerManager.Application.Extensions;
using ServerContainerManager.Application.Entities;

namespace ServerContainerManager.Application.Commands.Auth.SignIn
{
    internal class SignInCommandHandler(
        ILogger<SignInCommandHandler> logger,
        AppDbContext dbContext,
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager) : ICommandHandler<SignInCommand, SignInCommandResult>
    {
        private readonly ILogger<SignInCommandHandler> _logger = logger;
        private readonly AppDbContext _dbContext = dbContext;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<SignInCommandResult>> HandleAsync(SignInCommand command, CancellationToken cancellationToken = default)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            var user = await _userManager.GetUserByUsernameAsync(command.Username, cancellationToken);
            if (user == null)
                return Error.Unauthorized();

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

            var updateLastLoginResult = user.UpdateLastLogin(DateTime.UtcNow);
            if (updateLastLoginResult.IsError)
                return updateLastLoginResult.Errors;

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new SignInCommandResult();
        }
    }
}
