using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EstimationApplication.API.Authentication
{
    public class JwtHelper
    {
        public static JwtSecurityToken GetJwtToken(string username, string signingKey,
            string issuer, string audience, TimeSpan expiration, Claim[] additionalClaims = null)
        {
            var authClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                // this guarantees the token is unique
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (additionalClaims is object)
            {
                var claimList = new List<Claim>(authClaims);
                claimList.AddRange(additionalClaims);
                authClaims = claimList.ToArray();
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var credentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.UtcNow.Add(expiration),
                claims: authClaims,
                signingCredentials: credentials
            );
        }
    }
}
