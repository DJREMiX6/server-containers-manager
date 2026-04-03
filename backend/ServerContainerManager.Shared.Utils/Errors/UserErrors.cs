using ErrorOr;

namespace ServerContainerManager.Shared.Utils.Errors
{
    public static class UserErrors
    {
        private const string CodeKey = "User";

        public static Error UnauthorizedNotFound(Guid userId) => Error.Unauthorized($"{CodeKey}.{nameof(UnauthorizedNotFound)}", $"User {userId} not found.");
        public static Error CannotDeleteAdminUser() => Error.Forbidden($"{CodeKey}.{nameof(CannotDeleteAdminUser)}", "Cannot delete an Admin user.");
        public static Error SignInNotAllowed(string username) => Error.Forbidden($"{CodeKey}.{nameof(SignInNotAllowed)}", $"User {username} is not allowed to sign in.");
        public static Error LockedOut(string username) => Error.Forbidden($"{CodeKey}.{nameof(LockedOut)}", $"User {username} is locked out.");
        public static Error InvalidCredentials(string username) => Error.Unauthorized($"{CodeKey}.{nameof(InvalidCredentials)}", $"Invalid credentials for user {username}.");
    }
}
