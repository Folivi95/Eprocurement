using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using Newtonsoft.Json;

namespace EGPS.Application.Profiles
{
    public class UnitProfile: Profile
    {
        public UnitProfile()
        {
            CreateMap<Unit, UnitDTO>().AfterMap((src, dest) => { dest.CreatedAt = src.CreateAt; });

            CreateMap<UnitForCreationDTO, Unit>();

            CreateMap<UnitForUpdateDTO, Unit>().AfterMap((src, dest) => { dest.UpdatedAt = DateTime.Now; });

            CreateMap<UnitMember, UnitMemberDTO>().AfterMap((src, dest) =>
            {
                dest.Name = src.Unit.Name;
                dest.Id = src.Unit.Id;
            });

            CreateMap<UnitMember, UnitUserDTO>().AfterMap((src, dest) =>
            {
                dest.Id = src.User.Id;
                dest.FirstName = src.User.FirstName;
                dest.LastName  = src.User.LastName;
                dest.ProfilePicture = string.IsNullOrEmpty(src.User.ProfilePicture)
                    ? null
                    : JsonConvert.DeserializeObject(src.User.ProfilePicture);
            });

            CreateMap<Unit, UnitsDTO>().AfterMap((src, dest) => { dest.CreatedAt = src.CreateAt; });
        }
    }
}
