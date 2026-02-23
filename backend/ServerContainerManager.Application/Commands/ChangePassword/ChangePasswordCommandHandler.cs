using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Commands.ChangePassword
{
    internal class ChangePasswordCommandHandler(ILogger<ChangePasswordCommandHandler> logger, UserManager<AppUser> userManager) : ICommandHandler<ChangePasswordCommand, ChangePasswordCommandResult>
    {
        private readonly ILogger<ChangePasswordCommandHandler> _logger = logger;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ChangePasswordCommandResult> HandleAsync(ChangePasswordCommand command, CancellationToken cancellationToken = default)
        {
            if(command.CallerUserId != command.UserId)
                return await ChangeAnotherUserPassword(command, cancellationToken);

            return await ChangeCurrentUserPassword(command, cancellationToken);
        }

        private async Task<ChangePasswordCommandResult> ChangeAnotherUserPassword(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var callerUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == command.CallerUserId, cancellationToken);
            if (callerUser is null)
                return new ChangePasswordCommandResult([Error.Unauthorized($"{nameof(ChangePasswordCommandHandler)}.{nameof(ChangeAnotherUserPassword)}", $"The CallerUser {command.CallerUserId} does not exist")]);

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);
            if (user is null)
                return new ChangePasswordCommandResult([Error.Unauthorized($"{nameof(ChangePasswordCommandHandler)}.{nameof(ChangeAnotherUserPassword)}", $"The user {command.UserId} does not exist")]);

            if (!await _userManager.IsInRoleAsync(callerUser, UserRoles.Admin))
                return new ChangePasswordCommandResult([Error.Forbidden($"{nameof(ChangePasswordCommandHandler)}.{nameof(ChangeAnotherUserPassword)}", $"The CallerUser {callerUser.Id} does not have permission to change user {user.Id} password")]);

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword);
            if (!changePasswordResult.Succeeded)
                return new ChangePasswordCommandResult([.. changePasswordResult.Errors
                    .Select(e => Error.Validation($"{nameof(ChangePasswordCommandHandler)}.{nameof(ChangeAnotherUserPassword)}", e.Description))]);

            return new ChangePasswordCommandResult();
        }

        private async Task<ChangePasswordCommandResult> ChangeCurrentUserPassword(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);
            if (user is null)
                return new ChangePasswordCommandResult([Error.Unauthorized($"{nameof(ChangePasswordCommandHandler)}.{nameof(ChangeAnotherUserPassword)}", $"The user {command.UserId} does not exist")]);

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword);
            if (!changePasswordResult.Succeeded)
                return new ChangePasswordCommandResult([.. changePasswordResult.Errors
                    .Select(e => Error.Validation($"{nameof(ChangePasswordCommandHandler)}.{nameof(ChangeAnotherUserPassword)}", e.Description))]);

            return new ChangePasswordCommandResult();
        }
    }
}
