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

        public async Task<CreateUserCommandResult> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken = default)
        {
            var user = AppUser.Create(command.Username, []);

            var createResult = await _userManager.CreateAsync(user, command.Password);
            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();
                return new CreateUserCommandResult(errors);
            }

            var assignRoleResult = await _userManager.AddToRoleAsync(user, UserRoles.Member);
            if (!assignRoleResult.Succeeded)
            {
                var errors = assignRoleResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();
                return new CreateUserCommandResult(errors);
            }

            var userId = (await _userManager.FindByNameAsync(command.Username))!.Id;
            return new CreateUserCommandResult(userId);
        }
    }
}
