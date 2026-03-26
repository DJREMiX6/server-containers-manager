using ErrorOr;

namespace ServerContainerManager.Shared.Utils.Errors
{
    public static class UserErrors
    {
        public static Error UnauthorizedNotFound(Guid userId) => Error.Unauthorized("User.NotFound", $"User {userId} not found.");
        public static Error CannotDeleteAdminUser() => Error.Forbidden("User.CannotDeleteAdminUser", "Cannot delete an Admin user.");
        public static Error SignInNotAllowed(string username) => Error.Forbidden($"User.SignInNotAllowed", $"User {username} is not allowed to sign in.");
        public static Error LockedOut(string username) => Error.Forbidden("User.LockedOut", $"User {username} is locked out.");
        public static Error InvalidCredentials(string username) => Error.Unauthorized("User.InvalidCredentials", $"Invalid credentials for user {username}.");
    }
}
