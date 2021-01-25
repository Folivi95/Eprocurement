using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EGPS.Application.Models;
using EGPS.Domain.Entities;

namespace EGPS.Application.Profiles
{
    public class PasswordResetProfile: Profile
    {
        public PasswordResetProfile()
        {
            CreateMap<PasswordReset, PasswordResetDTO>();
            CreateMap<PasswordResetForCreationDTO, PasswordReset>();
        }
    }
}
