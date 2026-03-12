using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Commands.Abstraction;
using ServerContainerManager.Application.Consts;
using ServerContainerManager.Application.Entities;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Commands.CreateUser
{
    internal class CreateUserCommandHandler(ILogger<CreateUserCommandHandler> logger, AppDbContext dbContext, UserManager<AppUser> userManager) : ICommandHandler<CreateUserCommand, CreateUserCommandResult>
    {
        private readonly ILogger<CreateUserCommandHandler> logger = logger;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<ErrorOr<CreateUserCommandResult>> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken = default)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

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

            await transaction.CommitAsync(cancellationToken);

            return new CreateUserCommandResult(userId);
        }
    }
}
