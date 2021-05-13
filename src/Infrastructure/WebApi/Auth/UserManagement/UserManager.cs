using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Services.Users.Errors;
using Application.Services.Users.Repositories;
using Domain.Shared.Errors;
using Domain.Users.ValueObjects;

namespace WebApi.Auth.UserManagement
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository) =>
            _userRepository = userRepository;

        public async Task<UserId> GetUserId(ClaimsPrincipal claimsPrincipal)
        {
            var userId = ExtractUserIdFromClaims(claimsPrincipal);
            if (userId is not null)
                return userId;

            var emailAddress = ExtractEmailFromClaims(claimsPrincipal) ??
                               throw new ArgumentException("invalid claims");

            var owner = await _userRepository.GetByEmail(emailAddress) ??
                        throw new DomainException(new UserDoesNotExistError(emailAddress));
            return owner.Id;
        }

        private static UserId? ExtractUserIdFromClaims(ClaimsPrincipal claimsPrincipal)
        {
            var userIdClaim = GetClaim(claimsPrincipal, ClaimsConstants.UserIdClaim);
            return userIdClaim is not null ? new UserId(new Guid(userIdClaim)) : null;
        }


        private static EmailAddress? ExtractEmailFromClaims(ClaimsPrincipal claimsPrincipal)
        {
            var email = GetClaim(claimsPrincipal, ClaimsConstants.EmailClaim);
            return email is not null ? EmailAddress.Create(email) : null;
        }

        private static string? GetClaim(ClaimsPrincipal claimsPrincipal, string claim)
        {
            return claimsPrincipal.Claims.FirstOrDefault(t => claim.Equals(t.Type))?.Value;
        }
    }
}