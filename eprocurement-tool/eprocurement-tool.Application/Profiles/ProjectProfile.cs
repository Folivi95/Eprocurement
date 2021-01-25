using AutoMapper;
using EGPS.Application.Helpers;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;

namespace EGPS.Application.Profiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {

            CreateMap<Project, ProjectsDTO>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(s => s.CreateAt))
                .ForMember(d => d.RegistrationPlan, s => s.MapFrom(s => s.Contract.RegistrationPlan))
                .ForMember(d => d.Status, s => s.MapFrom(s => s.Status.GetDescription()));

            CreateMap<MilestoneTaskForCreateDTO, MilestoneTask>();

            CreateMap<MilestoneTask, MilestoneTaskDTO>()
                .ForMember(d => d.CreatedBy, s => s.MapFrom(s => s.CreatedById));

            CreateMap<MilestoneTaskForCreateDTO, MilestoneTask>()
                .ForMember(d => d.CreateAt, s => s.MapFrom(s => DateTime.Now));
            CreateMap<MilestoneTask, MilestoneTaskDTO>();

            CreateMap<Project, ProjectDetailsDTO>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(s => s.CreateAt));

        }
    }
}
