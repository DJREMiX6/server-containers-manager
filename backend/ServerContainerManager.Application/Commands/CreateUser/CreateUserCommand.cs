namespace ServerContainerManager.Application.Commands.CreateUser
{
    public record CreateUserCommand
    {
        public string Username { get; }
        public string Password { get; }

        public CreateUserCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
