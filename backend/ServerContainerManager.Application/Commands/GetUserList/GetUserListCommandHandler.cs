using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Commands.Models;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Commands.GetUserList
{
    internal class GetUserListCommandHandler(UserManager<AppUser> userManager) : ICommandHandler<GetUserListCommand, GetUserListCommandResult>
    {
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<GetUserListCommandResult>> HandleAsync(GetUserListCommand command, CancellationToken cancellationToken = default)
        {
            var users = await _userManager.Users
                .Include(u => u.Namespaces)
                .ToListAsync(cancellationToken);
            var userInfoList = new List<GetUserListCommandResultUserInfo>();

            users.ForEach(async u =>
            {
                var roles = await _userManager.GetRolesAsync(u);
                userInfoList.Add(new(
                    id: u.Id,
                    username: u.UserName!,
                    roles: roles,
                    namespaces: [.. u.Namespaces.Select(n => new NamespaceInfo(id: n.Id, name: n.Name))]));
            });

            return new GetUserListCommandResult(userInfoList);
        }
    }
}
