using AutoMapper;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;

namespace EGPS.Application.Profiles
{
    public class ResourceProfile : Profile
    {
        public ResourceProfile()
        {
            CreateMap<Resource, ResourceDTO>();
            CreateMap<ResourceForUpdateDTO, Resource>();
            CreateMap<RoleResourceForCreationDTO, Resource>();
        }
    }
}
