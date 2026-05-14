using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Extensions;
using ServerContainerManager.Application.Models;
using ServerContainerManager.Application.Queries.Abstraction;
using ServerContainerManager.Domain.Entities.Auth;
using ServerContainerManager.Shared.Utils.Errors;

namespace ServerContainerManager.Application.Queries.Auth.GetSessionInfo
{
    internal class GetSessionInfoCommandHandler(ILogger<GetSessionInfoCommandHandler> logger, AppDbContext appDbContext, UserManager<AppUser> userManager) : IQueryHandler<GetSessionInfoQuery, GetSessionInfoQueryResult>
    {
        private readonly ILogger<GetSessionInfoCommandHandler> _logger = logger;
        private readonly AppDbContext _appDbContext = appDbContext;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<GetSessionInfoQueryResult>> HandleAsync(GetSessionInfoQuery command, CancellationToken cancellationToken = default)
        {
            using var transaction = await _appDbContext.Database.BeginTransactionAsync(cancellationToken);

            var user = await _userManager.GetUserWithNamespacesAsync(command.UserId, cancellationToken);
            if (user == null)
                return UserErrors.UnauthorizedNotFound(command.UserId);

            var roles = await _userManager.GetRolesAsync(user);

            await transaction.CommitAsync(cancellationToken);

            return new GetSessionInfoQueryResult() 
            {
                User = new Models.User ()
                {
                    UserId = user.Id,
                    Username = user.UserName!,
                    Roles = roles,
                    Namespaces = [.. user.Namespaces.Select(NamespaceInfo.FromDomain)],
                    IsConfirmed = user.IsConfirmed,
                }
            };
        }
    }
}
