using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServerContainerManager.Application.Queries.Abstraction;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Queries.User.CheckUsernameAvailability
{
    internal sealed class CheckUsernameAvailabilityQueryHandler(
        ILogger<CheckUsernameAvailabilityQueryHandler> logger,
        UserManager<AppUser> userManager) : IQueryHandler<CheckUsernameAvailabilityQuery, CheckUsernameAvailabilityQueryResult>
    {
        private readonly ILogger<CheckUsernameAvailabilityQueryHandler> _logger = logger;
        private readonly UserManager<AppUser> _userManager = userManager;

        public async Task<ErrorOr<CheckUsernameAvailabilityQueryResult>> HandleAsync(CheckUsernameAvailabilityQuery command, CancellationToken cancellationToken = default)
        {
            var usernameExists = await _userManager.Users.AnyAsync(u => u.UserName == command.Username, cancellationToken);

            return new CheckUsernameAvailabilityQueryResult()
            { 
                IsAvailable = !usernameExists
            };
        }
    }
}
