namespace WebApi.Authorization
{
    public static class ClaimsConstants
    {
        
        private static string NameSpace = "https://todo-list-app.com/";
        public static string EmailClaim = $"{NameSpace}/email";
        public static string UserIdClaim = $"{NameSpace}/userId";
        public static string ScopeClaim = "scope";
    }
}