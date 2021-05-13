namespace WebApi.Auth
{
    public static class ClaimsConstants
    {
        private const string NameSpace = "https://todo-list-app.com/";
        public static readonly string EmailClaim = $"{NameSpace}email";
        public static readonly string UserIdClaim = $"{NameSpace}TodoListUserId";
        public const string ScopeClaim = "scope";
    }
}