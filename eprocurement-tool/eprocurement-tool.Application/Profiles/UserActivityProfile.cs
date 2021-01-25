using AutoMapper;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Profiles
{
    public class UserActivityProfile : Profile
    {
        public UserActivityProfile()
        {

            CreateMap<UserActivity, UserActivityDTO>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(s => s.CreatedAt)).ReverseMap();

            CreateMap<UserActivityForCreationDTO, UserActivity>();
        }
    }
}
