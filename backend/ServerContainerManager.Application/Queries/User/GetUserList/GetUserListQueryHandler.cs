using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Models;
using ServerContainerManager.Application.Queries.Abstraction;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Queries.User.GetUserList
{
    internal class GetUserListQueryHandler(ILogger<GetUserListQueryHandler> logger, AppDbContext appDbContext, UserManager<AppUser> userManager) : IQueryHandler<GetUserListQuery, GetUserListQueryResult>
    {
        private readonly ILogger<GetUserListQueryHandler> logger = logger;
        private readonly AppDbContext _appDbContext = appDbContext;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<GetUserListQueryResult>> HandleAsync(GetUserListQuery command, CancellationToken cancellationToken = default)
        {
            using var transaction = await _appDbContext.Database.BeginTransactionAsync(cancellationToken);

            var users = await _userManager.Users
                .Include(u => u.Namespaces)
                .ToListAsync(cancellationToken);
            var userInfoList = new List<GetUserListQueryUserResult>();

            users.ForEach(async u =>
            {
                var roles = await _userManager.GetRolesAsync(u);
                userInfoList.Add(new()
                {
                    Id = u.Id,
                    Username = u.UserName!,
                    Roles = roles,
                    Namespaces = [.. u.Namespaces.Select(NamespaceInfo.FromDomain)],
                    IsConfirmed = u.IsConfirmed,
                    LastLoginDate = u.LastLoginDate,
                });
            });

            await transaction.CommitAsync(cancellationToken);

            return new GetUserListQueryResult()
            {
                Users = userInfoList
            };
        }
    }
}
