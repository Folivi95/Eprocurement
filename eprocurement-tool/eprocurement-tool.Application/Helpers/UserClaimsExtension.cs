using System;
using System.Linq;
using System.Security.Claims;
using EGPS.Domain.Enums;

namespace EGPS.Application.Helpers
{
    public static class UserClaimsExtension
    {
        public static UserClaims UserClaims(this ClaimsPrincipal user)
        {
            
            var accountId = user.Claims.Where(x => x.Type == ClaimTypeHelper.AccountId).FirstOrDefault()?.Value;
            var userId = user.Claims.Where(x => x.Type == ClaimTypeHelper.UserId).FirstOrDefault()?.Value;
             var email = user.Claims.Where(x => x.Type == ClaimTypeHelper.Email).FirstOrDefault()?.Value;
            var role =  user.Claims.Where(x => x.Type == ClaimTypeHelper.Role).FirstOrDefault()?.Value;
            var userType = user.Claims.Where(x => x.Type == ClaimTypeHelper.UserType).FirstOrDefault()?.Value;
           
            return new UserClaims
            {
                AccountId = Guid.Parse(accountId),
                UserId = Guid.Parse(userId),
                Email = email,
                Role = (ERole)Enum.Parse(typeof(ERole), role, true),
                UserType = (EUserType)Enum.Parse(typeof(EUserType), userType, true)
            };
        }
    }

    public class UserClaims
    {
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public ERole? Role { get; set; }
        public EUserType UserType { get; set; }
    }
}
