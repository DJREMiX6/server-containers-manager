using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Extensions;
using ServerContainerManager.Domain.Entities.Auth;
using ServerContainerManager.Shared.Utils.Errors;

namespace ServerContainerManager.Application.Commands.User.ChangeUsername
{
    internal class ChangeUsernameCommandHandler(ILogger<ChangeUsernameCommandHandler> logger, AppDbContext dbContext, UserManager<AppUser> userManager) : ICommandHandler<ChangeUsernameCommand, ChangeUsernameCommandResult>
    {
        private readonly ILogger<ChangeUsernameCommandHandler> _logger = logger;
        private readonly AppDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<ChangeUsernameCommandResult>> HandleAsync(ChangeUsernameCommand command, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserByIdAsync(command.UserId, cancellationToken);
            if (user is null)
                return UserErrors.UnauthorizedNotFound(command.UserId);

            user.UserName = command.NewUsername;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return updateResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();

            return new ChangeUsernameCommandResult();
        }
    }
}
