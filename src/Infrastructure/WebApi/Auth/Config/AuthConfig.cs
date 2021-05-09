using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;

namespace WebApi.Auth.Config
{
    public class AuthConfig : IAuthConfig
    {
        private const string AuthIssuerParam = "AUTH_ISSUER";
        private const string AudienceParam = "AUTH_AUDIENCE";

        public AuthConfig(IConfiguration configuration)
        {
            Issuer = Guard.Against.NullOrEmpty(configuration[AuthIssuerParam], AuthIssuerParam);
            Audience = Guard.Against.NullOrEmpty(configuration[AudienceParam], AudienceParam);
        }

        public string GetIssuer() =>
            Issuer;

        public string Issuer { get; }

        public string GetAudience() =>
            Audience;

        public string Audience { get; }
    }
}