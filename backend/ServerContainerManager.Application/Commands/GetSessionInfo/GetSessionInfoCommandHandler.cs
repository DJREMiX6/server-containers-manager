using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Commands.Models;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Commands.GetSessionInfo
{
    internal class GetSessionInfoCommandHandler(ILogger<GetSessionInfoCommandHandler> logger, AppDbContext appDbContext, UserManager<AppUser> userManager) : ICommandHandler<GetSessionInfoCommand, GetSessionInfoCommandResult>
    {
        private readonly ILogger<GetSessionInfoCommandHandler> _logger = logger;
        private readonly AppDbContext _appDbContext = appDbContext;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<GetSessionInfoCommandResult>> HandleAsync(GetSessionInfoCommand command, CancellationToken cancellationToken = default)
        {
            using var transaction = await _appDbContext.Database.BeginTransactionAsync(cancellationToken);

            var user = await _userManager.Users
                .Where(u => u.Id == command.UserId)
                .Include(u => u.Namespaces)
                .FirstOrDefaultAsync(cancellationToken);
            if (user == null)
                return Error.NotFound($"{nameof(GetSessionInfoCommandHandler)}.{nameof(HandleAsync)}", $"User {command.UserId} not found");

            var roles = await _userManager.GetRolesAsync(user);

            await transaction.CommitAsync(cancellationToken);

            return new GetSessionInfoCommandResult(
                userId: user.Id, 
                username: user.UserName!, 
                roles: roles, 
                namespaces: [.. user.Namespaces
                    .Select(n => new NamespaceInfo(n.Id, n.Name))]);
        }
    }
}
