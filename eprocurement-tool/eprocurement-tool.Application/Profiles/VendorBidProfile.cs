using AutoMapper;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Profiles
{
    public class VendorBidProfile : Profile
    {
        public VendorBidProfile()
        {
            CreateMap<VendorBid, VendorBidForProcurementPlanDTO>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(s => s.CreateAt))
                .ForMember(d => d.VendorProfile, s => s.MapFrom(s => s.Vendor.VendorProfile));

            CreateMap<VendorProfile, VendorProfileForContractDTO>()
               .ForMember(d => d.CreatedAt, s => s.MapFrom(s => s.CreateAt));
        }
    }
}
