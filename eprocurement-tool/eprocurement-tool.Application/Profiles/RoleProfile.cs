using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EGPS.Application.Models;
using EGPS.Domain.Entities;

namespace EGPS.Application.Profiles
{
    public class RoleProfile: Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDTO>()
                .AfterMap((src, dest) => dest.CreatedAt = src.CreateAt);
            CreateMap<RoleForCreationDTO, Role>();
            CreateMap<RoleForUpdateDTO, Role>();
            CreateMap<Role, RoleResponse>();
        }
    }
}
