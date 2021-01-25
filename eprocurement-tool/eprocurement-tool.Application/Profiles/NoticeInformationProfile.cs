using AutoMapper;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Profiles
{
    public class NoticeInformationProfile : Profile
    {
        public NoticeInformationProfile()
        {
            CreateMap<NoticeInformation, NoticeInformationDTO>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(s => s.CreateAt))
                .ForMember(d => d.Description, s => s.MapFrom(s => s.Description));
        }
    }
}
