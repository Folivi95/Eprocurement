using AutoMapper;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Profiles
{
    public class BidProfile : Profile
    {
        public BidProfile()
        {
            CreateMap<Bid, BidDTO>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(s => s.CreateAt));

            CreateMap<ProcurementPlanActivity, ProcurementPlanActivityDTO>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(s => s.CreateAt)).ReverseMap();
        }
    }
}
