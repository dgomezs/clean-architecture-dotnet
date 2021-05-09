﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Domain.Users.ValueObjects;
using Microsoft.IdentityModel.Tokens;
using WebApi.Authorization;

namespace CleanArchitecture.TodoList.WebApi.Tests.Config
{
    public class FakeJwtManager
    {
        public static string Issuer { get; } = Guid.NewGuid().ToString();
        public static string Audience { get; } = Guid.NewGuid().ToString();
        public static SecurityKey SecurityKey { get; }
        public static SigningCredentials SigningCredentials { get; }

        private static readonly JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        private static readonly RandomNumberGenerator generator = RandomNumberGenerator.Create();
        private static readonly byte[] key = new byte[32];

        static FakeJwtManager()
        {
            generator.GetBytes(key);
            SecurityKey = new SymmetricSecurityKey(key) {KeyId = Guid.NewGuid().ToString()};
            SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
        }

        public static string GenerateJwtToken(EmailAddress userEmailAddress, List<string> scopes)
        {
            var claims = GetClaims(userEmailAddress, scopes);

            return tokenHandler.WriteToken(new JwtSecurityToken(Issuer, Audience, claims, null,
                DateTime.UtcNow.AddMinutes(10), SigningCredentials));
        }

        public static string GenerateExpiredJwtToken(EmailAddress userEmailAddress, List<string> scopes)
        {
            var claims = GetClaims(userEmailAddress, scopes);

            return tokenHandler.WriteToken(new JwtSecurityToken(Issuer, Audience, claims, null,
                DateTime.UtcNow.AddMinutes(-10), SigningCredentials));
        }

        private static List<Claim> GetClaims(EmailAddress userEmailAddress, List<string> scopes)
        {
            var claims = new List<Claim>
            {
                new Claim(type: ClaimsConstants.EmailClaim, value: userEmailAddress.Value,
                    ClaimValueTypes.String, Issuer),
                new Claim(type: "scope", string.Join(" ", scopes), ClaimValueTypes.String, Issuer)
            };
            return claims;
        }
    }
}