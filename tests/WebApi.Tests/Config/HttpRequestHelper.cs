using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Domain.Users.ValueObjects;
using Newtonsoft.Json;

namespace CleanArchitecture.TodoList.WebApi.Tests.Config
{
    public static class HttpRequestHelper
    {
        public static StringContent GetStringContent(object obj)
        {
            return new(JsonConvert.SerializeObject(obj), Encoding.Default, "application/json");
        }

        public static AuthenticationHeaderValue GetToken(EmailAddress ownerEmailAddress, List<string> scopes)
        {
            return GetAuthHeader(FakeJwtManager.GenerateJwtToken(ownerEmailAddress, scopes));
        }

        public static AuthenticationHeaderValue GetToken(UserId ownerId, List<string> scopes)
        {
            return GetAuthHeader(FakeJwtManager.GenerateJwtToken(ownerId, scopes));
        }

        public static AuthenticationHeaderValue GetExpiredToken(EmailAddress ownerEmailAddress,
            List<string> scopes)
        {
            return GetAuthHeader(FakeJwtManager.GenerateExpiredJwtToken(ownerEmailAddress, scopes));
        }

        public static AuthenticationHeaderValue GetAuthHeader(string token)
        {
            return new("Bearer", token);
        }
    }
}