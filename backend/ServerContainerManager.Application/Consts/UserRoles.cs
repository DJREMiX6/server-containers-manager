namespace ServerContainerManager.Application.Consts
{
    public static class UserRoles
    {
        public static string[] AllRoles => [Admin, Member];
        public const string Admin = "Admin";
        public const string Member = "Member";
        public const string AllRolesAuthorizeFormat = $"{Admin},{Member}";
    }
}
