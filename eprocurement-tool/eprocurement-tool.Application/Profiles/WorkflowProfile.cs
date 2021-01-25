using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using EGPS.Application.Models;
using EGPS.Domain.Entities;

namespace EGPS.Application.Profiles
{
    public class WorkflowProfile: Profile
    {
        public WorkflowProfile()
        {
            CreateMap<Workflow, WorkflowDTO>().AfterMap((src, dest) => { dest.CreatedAt = src.CreateAt; });
            CreateMap<Workflow, WorkflowForGetDTO>().AfterMap((src, dest) => { dest.CreatedAt = src.CreateAt; });
            CreateMap<WorkflowForCreationDTO, Workflow>();
            CreateMap<WorkflowForUpdateDTO, Workflow>();
            CreateMap<Workflow, WorkflowsDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
                dest.TotalStages = src.Stages.ToArray().Length;
            });
        }
    }
}
