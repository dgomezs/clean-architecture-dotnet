namespace WebApi.Auth
{
    public static class ClaimsConstants
    {
        private const string NameSpace = "https://todo-list-app.com/";
        public const string ScopeClaim = "scope";
        public static readonly string EmailClaim = $"{NameSpace}email";
        public static readonly string UserIdClaim = $"{NameSpace}TodoListUserId";
    }
}