// HasScopeRequirement.cs

using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Auth.Scopes
{
    public class HasScopeRequirement : IAuthorizationRequirement
    {
        public string Issuer { get; }
        public string Scope { get; }

        public HasScopeRequirement(string scope, string issuer)
        {
            Scope = Guard.Against.NullOrEmpty(scope, nameof(scope));
            Issuer = Guard.Against.NullOrEmpty(issuer, nameof(issuer));
        }
    }
}