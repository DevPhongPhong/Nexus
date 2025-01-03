﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Nexus.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EIM.Attributes.FilterPipelines.Authorizations
{
    public static class NexusAuthorizationHelpers
    {
        /// <summary>
        /// Generate token for an employee.
        /// </summary>
        public static string GenerateToken(Employee employee, string secretKey, DateTime expiredTime)
        {
            var claims = new List<Claim>
        {
            new Claim("Employee", JsonConvert.SerializeObject(employee)),
            new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(expiredTime).ToUnixTimeSeconds().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiredTime,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static ClaimsPrincipal? ValidateToken(string token, string secretKey)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            try
            {
                // Validate the token and retrieve claims principal
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                // Ensure token is a valid JWT
                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    // Optional: Perform additional checks on claims (if needed)
                    var employeeClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Employee");

                    // Validate essential claims
                    if (employeeClaim == null)
                    {
                        return null;
                    }
                }

                return principal;
            }
            catch
            {
                // Log or handle token validation failures if necessary
                return null;
            }
        }

        /// <summary>
        /// Check if the action allows anonymous requests.
        /// </summary>
        public static bool IsAllowAnonymousRequest(AuthorizationFilterContext context)
        {
            return context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute));
        }
    }
}
