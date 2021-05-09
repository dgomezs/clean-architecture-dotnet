﻿using System.Linq;
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
            var emailAddress = GetEmail(claimsPrincipal);

            var owner = await _userRepository.GetByEmail(emailAddress) ??
                        throw new DomainException(new UserDoesNotExistError(emailAddress));
            return owner.Id;
        }

        public Task<bool> HasUserSignedUpInAuthSystem(EmailAddress create)
        {
            return Task.FromResult(true);
        }

        private static EmailAddress GetEmail(ClaimsPrincipal claimsPrincipal)
        {
            var email = GetClaim(claimsPrincipal, ClaimsConstants.EmailClaim);
            return EmailAddress.Create(email);
        }

        private static string? GetClaim(ClaimsPrincipal claimsPrincipal, string claim)
        {
            return claimsPrincipal.Claims.FirstOrDefault(t => claim.Equals(t.Type))?.Value;
        }
    }
}