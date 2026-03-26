using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Extensions;
using ServerContainerManager.Application.Models;
using ServerContainerManager.Domain.Entities.Auth;
using ServerContainerManager.Shared.Utils.Errors;

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

            var user = await _userManager.GetUserWithNamespacesAsync(command.UserId, cancellationToken);
            if (user == null)
                return UserErrors.UnauthorizedNotFound(command.UserId);

            var roles = await _userManager.GetRolesAsync(user);

            await transaction.CommitAsync(cancellationToken);

            return new GetSessionInfoCommandResult(
                userId: user.Id, 
                username: user.UserName!, 
                roles: roles, 
                namespaces: [.. user.Namespaces
                    .Select(NamespaceInfo.FromDomain)]);
        }
    }
}
