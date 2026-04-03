using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Extensions;
using ServerContainerManager.Domain.Entities.Auth;
using ServerContainerManager.Shared.Utils.Errors;

namespace ServerContainerManager.Application.Commands.User.DeleteUser
{
    internal class DeleteUserCommandHandler(ILogger<DeleteUserCommandHandler> logger, AppDbContext appDbContext, UserManager<AppUser> userManager) : ICommandHandler<DeleteUserCommand, DeleteUserCommandResult>
    {
        private readonly ILogger<DeleteUserCommandHandler> _logger = logger;
        private readonly AppDbContext _appDbContext = appDbContext = appDbContext;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<DeleteUserCommandResult>> HandleAsync(DeleteUserCommand command, CancellationToken cancellationToken = default)
        {
            using var transaction = await _appDbContext.Database.BeginTransactionAsync(cancellationToken);

            var user = await _userManager.GetUserByIdAsync(command.UserId, cancellationToken);
            if (user is null)
                return UserErrors.UnauthorizedNotFound(command.UserId);

            if (await _userManager.IsInRoleAsync(user, UserRoles.Admin))
                return UserErrors.CannotDeleteAdminUser();

            var securityStampUpdateResult = await _userManager.UpdateSecurityStampAsync(user); // Forces user logout
            if (!securityStampUpdateResult.Succeeded)
                return securityStampUpdateResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();

            var deleteResult = await _userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
                return deleteResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();

            await transaction.CommitAsync(cancellationToken);

            return new DeleteUserCommandResult();
        }
    }
}
