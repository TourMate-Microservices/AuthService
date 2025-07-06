using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TourMate.AuthService.Repositories.Models;
using TourMate.AuthService.Services.IServices;

namespace TourMate.AuthService.Services.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenService _refreshTokenService;

        public TokenService(IConfiguration configuration, IRefreshTokenService refreshTokenService)
        {
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
        }

        public string GenerateAccessToken(int accountId, string fullName, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"] ?? "default-secret-key");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("AccountId", accountId.ToString()),
                    new Claim("FullName", fullName),
                    new Claim("Role", role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:AccessTokenExpirationMinutes"] ?? "60")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> GenerateAndSaveRefreshTokenAsync(int accountId)
        {
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                UserId = accountId,
                ExpireAt = DateTime.UtcNow.AddDays(double.Parse(_configuration["JwtSettings:RefreshTokenExpirationDays"] ?? "7")),
                IsRevoked = false
            };

            await _refreshTokenService.CreateRefreshToken(refreshToken);
            return refreshToken.Token;
        }

        public string GenerateResetPasswordToken(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"] ?? "default-secret-key");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("AccountId", account.AccountId.ToString()),
                    new Claim("Email", account.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token valid for 1 hour
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal? ValidateResetPasswordToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"] ?? "default-secret-key");
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
