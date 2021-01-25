using AutoMapper;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using EGPS.Application.Helpers;
using EGPS.Application.Models.StaffModels;

namespace EGPS.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
                dest.Role = src.Role == null ? null : src.Role.GetDescription();
                dest.ProfilePicture = string.IsNullOrEmpty(src.ProfilePicture) ? null : JsonConvert.DeserializeObject(src.ProfilePicture);
            });

            CreateMap<User, UsersDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
                dest.Role = src.Role == null ? null : src.Role.GetDescription();
                dest.ProfilePicture = string.IsNullOrEmpty(src.ProfilePicture)
                    ? null
                    : JsonConvert.DeserializeObject(src.ProfilePicture);
            });

            CreateMap<UserForCreationDTO, User>()
                .ForMember(
                    dest => dest.VerificationToken,
                    opt => opt.MapFrom(src => src.Token));

            CreateMap<UserForUpdateDTO, User>().AfterMap((src, dest) => {
                dest.UpdatedAt = DateTime.Now;
                dest.Gender = src.Gender.ToLower();
            });

            CreateMap<User, UserMemberDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
                dest.ProfilePicture = string.IsNullOrEmpty(src.ProfilePicture)
                    ? null
                    : JsonConvert.DeserializeObject(src.ProfilePicture);
            });

            CreateMap<DepartmentMember, DepartmentUser>().AfterMap((src, dest) =>
            {
                dest.Id = src.Department.Id;
                dest.Name = src.Department.Name;
            });

            CreateMap<UnitMember, UnitUser>().AfterMap((src, dest) =>
            {
                dest.Id   = src.Unit.Id;
                dest.Name = src.Unit.Name;
            });

            CreateMap<UserRole, UserRoleDTO>().AfterMap((src, dest) =>
            {
                dest.Id = src.Role.Id;
                dest.Title = src.Role.Title;
                dest.Description = src.Role.Description;
                dest.CreatedAt = src.Role.CreateAt;
            });

            CreateMap<User, ReviewUser>()
                .ForMember(dest => dest.ProfilePicture, src => src.MapFrom(s => string.IsNullOrEmpty(s.ProfilePicture) ? null : JsonConvert.DeserializeObject(s.ProfilePicture)));

            CreateMap<User, StaffWithTokenDto>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
                dest.Role = src.Role == null ? null : src.Role.GetDescription();
                dest.ProfilePicture = string.IsNullOrEmpty(src.ProfilePicture)
                    ? null
                    : JsonConvert.DeserializeObject(src.ProfilePicture);
            });

        }
    }
}
