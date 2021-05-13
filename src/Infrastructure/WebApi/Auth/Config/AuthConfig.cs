using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;

namespace WebApi.Auth.Config
{
    public class AuthConfig : IAuthConfig
    {
        private readonly string _audience;
        private readonly string _issuer;
        private const string AuthIssuerParam = "AUTH_ISSUER";
        private const string AudienceParam = "AUTH_AUDIENCE";

        public AuthConfig(IConfiguration configuration)
        {
            _issuer = Guard.Against.NullOrEmpty(configuration[AuthIssuerParam], AuthIssuerParam);
            _audience = Guard.Against.NullOrEmpty(configuration[AudienceParam], AudienceParam);
        }

        public string GetIssuer() =>
            _issuer;

        public string GetAudience() =>
            _audience;
    }
}