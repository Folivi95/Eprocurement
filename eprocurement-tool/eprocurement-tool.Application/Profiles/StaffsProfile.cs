using AutoMapper;
using EGPS.Application.Models;
using EGPS.Application.Models.StaffModels;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Helpers;
using Newtonsoft.Json;

namespace EGPS.Application.Profiles
{
    public class StaffsProfile : Profile
    {
        public StaffsProfile()
        {
            CreateMap<User, StaffUserDto>()
                .ForMember(dest => dest.Id, src => src.MapFrom(s => s.Id))
                .ForMember(dest => dest.CreatedAt, src => src.MapFrom(s => s.CreateAt));
            CreateMap<User, StaffUserDto>().AfterMap((src, dest) =>
            {
             
                dest.Role = src.Role == null ? null : src.Role.GetDescription();
                dest.ProfilePicture = string.IsNullOrEmpty(src.ProfilePicture) ? null : JsonConvert.DeserializeObject(src.ProfilePicture);
            });

            CreateMap<Ministry, MinistryDTO>()
                .ForMember(dest => dest.CreatedAt, src => src.MapFrom(x => x.CreateAt));
            CreateMap<UserRole, UserRoleDTO>()
                .ForMember(dest => dest.CreatedAt, src => src.MapFrom(x => x.CreateAt));
        }
    }
}
