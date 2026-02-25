using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Commands.ChangeUsername
{
    internal class ChangeUsernameCommandHandler(ILogger<ChangeUsernameCommandHandler> logger, AppDbContext dbContext, UserManager<AppUser> userManager) : ICommandHandler<ChangeUsernameCommand, ChangeUsernameCommandResult>
    {
        private readonly ILogger<ChangeUsernameCommandHandler> _logger = logger;
        private readonly AppDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<ChangeUsernameCommandResult>> HandleAsync(ChangeUsernameCommand command, CancellationToken cancellationToken = default)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            
            var user = await _userManager.Users
                .Where(u => u.Id == command.UserId)
                .FirstOrDefaultAsync(cancellationToken);
            if (user is null)
                return Error.NotFound($"{nameof(ChangeUsernameCommandHandler)}.{nameof(HandleAsync)}", $"Cannot find user {command.UserId}");

            user.UserName = command.NewUsername;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return updateResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();

            await transaction.CommitAsync(cancellationToken);

            return new ChangeUsernameCommandResult();
        }
    }
}
