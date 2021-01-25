using AutoMapper;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Profiles
{
    public class MinistryProfile : Profile
    {
        public MinistryProfile()
        {
            CreateMap<Ministry, MinistryDTO>()
                .ForMember(dest => dest.CreatedAt, src => src.MapFrom(s => s.CreateAt)).ReverseMap();

            CreateMap<Ministry, MinistryPlan>();
        }
    }
}
