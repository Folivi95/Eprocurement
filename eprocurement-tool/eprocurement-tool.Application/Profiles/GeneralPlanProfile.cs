using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EGPS.Application.Models;
using EGPS.Application.Repository;
using EGPS.Domain.Entities;

namespace EGPS.Application.Profiles
{
    class GeneralPlanProfile: Profile
    {
        public GeneralPlanProfile()
        {
            CreateMap<GeneralPlanForCreation, GeneralPlan>();
            CreateMap<GeneralPlan, GeneralPlanDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
            });
            CreateMap<GeneralPlanForUpdate, GeneralPlan>();
        }
    }
}
