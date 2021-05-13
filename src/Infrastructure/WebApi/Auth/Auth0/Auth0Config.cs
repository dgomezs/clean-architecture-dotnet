using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;

namespace WebApi.Auth.Auth0
{
    public class Auth0Config
    {
        public Auth0Config(IConfiguration configuration)
        {
            ClientId =
                Guard.Against.NullOrEmpty(configuration["AUTH0_CLIENT_ID"], "AUTH0_CLIENT_ID");
            ClientSecret =
                Guard.Against.NullOrEmpty(configuration["AUTH0_CLIENT_SECRET"], "AUTH0_CLIENT_SECRET");
            Domain = Guard.Against.NullOrEmpty(configuration["AUTH_ISSUER"],
                "AUTH_ISSUER");
            TokenPath = $"{Domain}oauth/token";
            TokenAudience = $"{Domain}api/v2/";
            ManagementApi = $"{Domain}api/v2";
        }

        public string ManagementApi { get; }

        public string TokenPath { get; }

        public string ClientSecret { get; }

        public string Domain { get; }

        public string ClientId { get; }
        public string TokenAudience { get; }
    }
}