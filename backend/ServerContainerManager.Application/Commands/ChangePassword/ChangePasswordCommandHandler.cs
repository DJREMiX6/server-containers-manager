using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Commands.ChangePassword
{
    internal class ChangePasswordCommandHandler(ILogger<ChangePasswordCommandHandler> logger, AppDbContext dbContext, UserManager<AppUser> userManager) : ICommandHandler<ChangePasswordCommand, ChangePasswordCommandResult>
    {
        private readonly ILogger<ChangePasswordCommandHandler> _logger = logger;
        private readonly AppDbContext _dbContext = dbContext;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<ChangePasswordCommandResult>> HandleAsync(ChangePasswordCommand command, CancellationToken cancellationToken = default)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);
            if (user is null)
                return Error.Unauthorized($"{nameof(ChangePasswordCommandHandler)}.{nameof(HandleAsync)}", $"The user {command.UserId} does not exist");

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword);
            if (!changePasswordResult.Succeeded)
                return changePasswordResult.Errors
                    .Select(e => Error.Validation($"{nameof(ChangePasswordCommandHandler)}.{nameof(HandleAsync)}", e.Description))
                    .ToList();

            await transaction.CommitAsync(cancellationToken);

            return new ChangePasswordCommandResult();
        }
    }
}
