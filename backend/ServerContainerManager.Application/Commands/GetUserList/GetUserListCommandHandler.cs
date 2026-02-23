using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Commands.GetUserList
{
    internal class GetUserListCommandHandler(UserManager<AppUser> userManager) : ICommandHandler<GetUserListCommand, IEnumerable<GetUserListCommandResult>>
    {
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<IEnumerable<GetUserListCommandResult>> HandleAsync(GetUserListCommand command, CancellationToken cancellationToken = default)
        {
            var users = await _userManager.Users
                .Include(u => u.Namespaces)
                .ToListAsync(cancellationToken);
            var result = new List<GetUserListCommandResult>();

            users.ForEach(async u =>
            {
                var roles = await _userManager.GetRolesAsync(u);
                result.Add(new GetUserListCommandResult(
                    Id: u.Id,
                    Username: u.UserName!,
                    Roles: roles,
                    Namespaces: u.Namespaces.Select(n => new GetUserListCommandResultNamespace(Id: n.Id, Name: n.Name))));
            });

            return result;
        }
    }
}
