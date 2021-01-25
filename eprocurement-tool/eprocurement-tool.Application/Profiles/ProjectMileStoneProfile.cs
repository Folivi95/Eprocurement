using AutoMapper;
using EGPS.Application.Helpers;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Profiles
{
    public class ProjectMileStoneProfile : Profile
    {
        public ProjectMileStoneProfile()
        {
            CreateMap<ProjectMileStoneForCreationDTO, ProjectMileStone>();
            CreateMap<ProjectMileStone, ProjectMileStoneDTO>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(s => s.CreateAt))
                .ForMember(d => d.Status, s => s.MapFrom(s => s.Status.GetDescription()));
            CreateMap<ProjectMileStone, TransactionProjectMilestoneDTO>();

            CreateMap<MilestoneTask, MilestoneTaskDTO>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(s => s.CreateAt))
                .ForMember(d => d.Status, s => s.MapFrom(s => s.Status.GetDescription()));

            CreateMap<MilestoneInvoice, MilestoneInvoiceDTO>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(s => s.CreateAt));


        }
    }
}
