using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Extensions;
using ServerContainerManager.Domain.Entities.Auth;
using ServerContainerManager.Shared.Utils.Errors;

namespace ServerContainerManager.Application.Commands.User.ChangePassword
{
    internal class ChangePasswordCommandHandler(ILogger<ChangePasswordCommandHandler> logger, AppDbContext dbContext, UserManager<AppUser> userManager) : IQueryHandler<ChangePasswordCommand, ChangePasswordCommandResult>
    {
        private readonly ILogger<ChangePasswordCommandHandler> _logger = logger;
        private readonly AppDbContext _dbContext = dbContext;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<ChangePasswordCommandResult>> HandleAsync(ChangePasswordCommand command, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserByIdAsync(command.UserId, cancellationToken);
            if (user is null)
                return UserErrors.UnauthorizedNotFound(command.UserId);

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword);
            if (!changePasswordResult.Succeeded)
                return changePasswordResult.Errors.ToError().ToList();

            if(!user.IsConfirmed)
            {
                var confirmResult = user.Confirm();
                if (confirmResult.IsError)
                    return confirmResult.Errors;
            }

            await _userManager.UpdateSecurityStampAsync(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new ChangePasswordCommandResult();
        }
    }
}
