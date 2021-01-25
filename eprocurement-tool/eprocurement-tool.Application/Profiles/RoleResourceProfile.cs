using AutoMapper;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Profiles
{
    public class RoleResourceProfile : Profile
    {
        public RoleResourceProfile()
        {
            CreateMap<RoleResource, RoleResourceDTO>()
                .AfterMap((src, dest) =>
                {
                    dest.Id = src.ResourceId;
                    dest.Name = src.Resource.Name;
                    dest.Permissions = string.IsNullOrEmpty(src.Permissions) ? null : JsonConvert.DeserializeObject(src.Permissions);
                });
            CreateMap<RoleResourceForCreationDTO, RoleResource>()
                .AfterMap((src, dest) =>
                {
                    dest.Permissions = src.Permissions == null ? string.Empty : JsonConvert.SerializeObject(src.Permissions);
                });
        }
    }
}
