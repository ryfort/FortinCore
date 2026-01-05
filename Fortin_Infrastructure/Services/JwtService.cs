using Fortin.Infrastructure.Entities;
using Fortin.Infrastructure.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Fortin.Common.Configuration;

namespace Fortin.Infrastructure.Services
{
    public class JwtService(IOptionsMonitor<JwtOptions> jwtSettings) : IJwtService
    {
        private readonly JwtOptions _jwtOptions = jwtSettings.CurrentValue;

        public string GenerateToken(User user)
        {
            var secretKey = _jwtOptions.SecretKey;
            var issuer = _jwtOptions.Issuer;
            var audience = _jwtOptions.Audience;
            var expirationMinutes = _jwtOptions.ExpirationMinutes ?? 60;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                //new Claim(ClaimTypes.Email, user.Email),
                new Claim("username", user.Username),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                //new Claim(ClaimTypes.Role, user.Role.ToString()),
                //new Claim("role", user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public int GetTokenExpirationMinutes()
        {
            return _jwtOptions.ExpirationMinutes?? 60;
        }

        public string? ValidateToken(string token)
        {
            try
            {
                var secretKey = _jwtOptions.SecretKey;
                var issuer = _jwtOptions.Issuer;
                var audience = _jwtOptions.Audience;

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
                var tokenHandler = new JwtSecurityTokenHandler();

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                return userId;
            }
            catch
            {
                return null;
            }
        }
    }
}
