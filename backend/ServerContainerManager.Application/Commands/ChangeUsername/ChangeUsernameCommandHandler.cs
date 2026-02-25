using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Commands.ChangeUsername
{
    internal class ChangeUsernameCommandHandler(UserManager<AppUser> userManager) : ICommandHandler<ChangeUsernameCommand, ChangeUsernameCommandResult>
    {
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<ChangeUsernameCommandResult>> HandleAsync(ChangeUsernameCommand command, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users
                .Where(u => u.Id == command.UserId)
                .FirstOrDefaultAsync(cancellationToken);
            if (user is null)
                return Error.NotFound($"{nameof(ChangeUsernameCommandHandler)}.{nameof(HandleAsync)}", $"Cannot find user {command.UserId}");

            user.UserName = command.NewUsername;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return updateResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();

            return new ChangeUsernameCommandResult();
        }
    }
}
