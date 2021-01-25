using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using Newtonsoft.Json;

namespace EGPS.Application.Profiles
{
    public class DepartmentProfile: Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDTO>()
                .AfterMap((src, dest) => dest.CreatedAt = src.CreateAt);

            CreateMap<DepartmentForCreationDTO, Department>();

            CreateMap<DepartmentForUpdateDTO, Department>()
                .AfterMap((src,dest) => dest.UpdatedAt = DateTime.Now);
        }
    }
}
