using EGPS.Application.Helpers;
using EGPS.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EGPS.Application.Interfaces
{
    public class JwtAuthenticateManager : IJwtAuthenticationManager
    {
        public JwtAuthenticateManager(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public (string Token, DateTime? ExpiresIn) Authenticate(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration["TOKEN_SECRETS"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypeHelper.Email, user.Email),
                    new Claim(ClaimTypeHelper.UserId, user.Id.ToString()),
                    new Claim(ClaimTypeHelper.AccountId, user.AccountId.ToString()),
                    new Claim(ClaimTypeHelper.Role, user.Role.ToString()),
                    new Claim(ClaimTypeHelper.UserType, user.UserType.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return (jwt, tokenDescriptor.Expires);
        } 
    }
}
