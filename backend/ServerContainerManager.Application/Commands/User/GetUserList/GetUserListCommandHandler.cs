using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Application.Models;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Commands.User.GetUserList
{
    internal class GetUserListCommandHandler(ILogger<GetUserListCommandHandler> logger, AppDbContext appDbContext, UserManager<AppUser> userManager) : ICommandHandler<GetUserListCommand, GetUserListCommandResult>
    {
        private readonly ILogger<GetUserListCommandHandler> logger = logger;
        private readonly AppDbContext _appDbContext = appDbContext;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<GetUserListCommandResult>> HandleAsync(GetUserListCommand command, CancellationToken cancellationToken = default)
        {
            using var transaction = await _appDbContext.Database.BeginTransactionAsync(cancellationToken);

            var users = await _userManager.Users
                .Include(u => u.Namespaces)
                .ToListAsync(cancellationToken);
            var userInfoList = new List<GetUserListCommandResultUserInfo>();

            users.ForEach(async u =>
            {
                var roles = await _userManager.GetRolesAsync(u);
                userInfoList.Add(new()
                {
                    Id = u.Id,
                    Username = u.UserName!,
                    Roles = roles,
                    Namespaces = [.. u.Namespaces.Select(NamespaceInfo.FromDomain)]
                });
            });

            await transaction.CommitAsync(cancellationToken);

            return new GetUserListCommandResult()
            {
                Users = userInfoList
            };
        }
    }
}
