using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Commands.DeleteUser
{
    internal class DeleteUserCommandHandler(UserManager<AppUser> userManager) : ICommandHandler<DeleteUserCommand, DeleteUserCommandResult>
    {
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<DeleteUserCommandResult>> HandleAsync(DeleteUserCommand command, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users
                .Where(u => u.Id == command.UserId)
                .FirstOrDefaultAsync(cancellationToken);
            if (user is null)
                return Error.NotFound($"{nameof(DeleteUserCommandHandler)}.{nameof(HandleAsync)}", $"Cannot find user {command.UserId}");

            if (await _userManager.IsInRoleAsync(user, UserRoles.Admin))
                return Error.Forbidden($"{nameof(DeleteUserCommandHandler)}.{nameof(HandleAsync)}", "Cannot delete an Admin user");

            var securityStampUpdateResult = await _userManager.UpdateSecurityStampAsync(user); // Forces user logout
            if (!securityStampUpdateResult.Succeeded)
                return securityStampUpdateResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();

            var deleteResult = await _userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
                return deleteResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();

            return new DeleteUserCommandResult();
        }
    }
}
