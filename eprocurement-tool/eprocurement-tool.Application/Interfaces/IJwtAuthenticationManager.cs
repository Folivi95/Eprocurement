using EGPS.Domain.Entities;
using System;

namespace EGPS.Application.Interfaces
{
    public interface IJwtAuthenticationManager
    {
        (string Token, DateTime? ExpiresIn) Authenticate(User user);
    }
}
