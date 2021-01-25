using AutoMapper;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Profiles
{
    public class UserInvitationProfile : Profile
    {
        public UserInvitationProfile()
        {
            CreateMap<UserInvitation, UserInvitationDTO>()
                .ForMember(dest => dest.CreatedAt, src => src.MapFrom(x => x.CreateAt));
            CreateMap<UserInvitationForCreationDTO, UserInvitation>();
        }
    }
}
