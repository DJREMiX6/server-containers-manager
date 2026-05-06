using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Extensions;
using ServerContainerManager.Domain.Entities.Auth;
using ServerContainerManager.Shared.Utils.Errors;

namespace ServerContainerManager.Application.Commands.User.ResetPassword
{
    internal class ResetUserPasswordCommandHandler(
        ILogger<ResetUserPasswordCommandHandler> logger,
        AppDbContext dbContext,
        UserManager<AppUser> userManager) : ICommandHandler<ResetUserPasswordCommand, ResetUserPasswordCommandResult>
    {
        private readonly ILogger<ResetUserPasswordCommandHandler> _logger = logger;
        private readonly AppDbContext _dbContext = dbContext;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<ResetUserPasswordCommandResult>> HandleAsync(ResetUserPasswordCommand command, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.FindAsync([command.UserId], cancellationToken);
            if (user is null)
                return UserErrors.NotFound(command.UserId);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, command.Password);

            if (!result.Succeeded)
                return result.Errors.ToError().ToList();

            if(user.IsConfirmed)
            {
                var unconfirmUserResult = user.Unconfirm();
                if (unconfirmUserResult.IsError)
                    return unconfirmUserResult.Errors;
            }

            await _userManager.UpdateSecurityStampAsync(user);
            await _userManager.UpdateAsync(user);

            return new ResetUserPasswordCommandResult();
        }
    }
}
