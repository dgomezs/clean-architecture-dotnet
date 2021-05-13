﻿namespace WebApi.Auth
{
    public static class ClaimsConstants
    {
        
        private static string NameSpace = "https://todo-list-app.com/";
        public static string EmailClaim = $"{NameSpace}/email";
        public static string UserIdClaim = $"{NameSpace}/TodoListUserId";
        public static string ScopeClaim = "scope";
    }
}