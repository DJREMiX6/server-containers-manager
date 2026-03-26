using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Commands.CreateUser
{
    internal class CreateUserCommandHandler(ILogger<CreateUserCommandHandler> logger, UserManager<AppUser> userManager) : ICommandHandler<CreateUserCommand, CreateUserCommandResult>
    {
        private readonly ILogger<CreateUserCommandHandler> logger = logger;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<CreateUserCommandResult>> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken = default)
        {
            var createUserResult = AppUser.Create(command.Username, []);
            if (createUserResult.IsError)
                return createUserResult.Errors;

            var createResult = await _userManager.CreateAsync(createUserResult.Value, command.Password);
            if (!createResult.Succeeded)
                return createResult.Errors
                    .Select(e => Error.Validation(e.Code, e.Description))
                    .ToList();

            var assignRoleResult = await _userManager.AddToRoleAsync(createUserResult.Value, UserRoles.Member);
            if (!assignRoleResult.Succeeded)
                return assignRoleResult.Errors
                    .Select(e => Error.Validation(e.Code, e.Description))
                    .ToList();

            var userId = (await _userManager.FindByNameAsync(command.Username))!.Id;

            return new CreateUserCommandResult() { UserId = userId };
        }
    }
}
