using ErrorOr;
using Microsoft.AspNetCore.Identity;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Commands.CreateUser
{
    internal class CreateUserCommandHandler(UserManager<AppUser> userManager) : ICommandHandler<CreateUserCommand, CreateUserCommandResult>
    {
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<CreateUserCommandResult>> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken = default)
        {
            var user = AppUser.Create(command.Username, []);

            var createResult = await _userManager.CreateAsync(user, command.Password);
            if (!createResult.Succeeded)
                return createResult.Errors
                    .Select(e => Error.Validation(e.Code, e.Description))
                    .ToList();

            var assignRoleResult = await _userManager.AddToRoleAsync(user, UserRoles.Member);
            if (!assignRoleResult.Succeeded)
                return assignRoleResult.Errors
                    .Select(e => Error.Validation(e.Code, e.Description))
                    .ToList();

            var userId = (await _userManager.FindByNameAsync(command.Username))!.Id;
            return new CreateUserCommandResult(userId);
        }
    }
}
