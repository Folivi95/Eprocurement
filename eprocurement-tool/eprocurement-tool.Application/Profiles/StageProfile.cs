using System;
using AutoMapper;
using EGPS.Application.Helpers;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using System.Linq;

namespace EGPS.Application.Profiles
{
    public class StageProfile : Profile
    {
        
        public StageProfile()
        {
            CreateMap<Stage, StageDTO>()
                .AfterMap((src, dest) =>
                {
                    dest.Action     = src.Action.GetDescription();
                    dest.GroupClass = string.IsNullOrEmpty(src.GroupIds) ? null : src.GroupClass.GetDescription();
                    dest.UserType   = src.UserType.GetDescription();
                    dest.AssigneeIds = string.IsNullOrEmpty(src.AssigneeIds) ? new string[] { } : src.AssigneeIds.Split(',').Select(p => p.Trim()).ToArray();
                    dest.GroupIds = string.IsNullOrEmpty(src.GroupIds) ? new string[] { } : src.GroupIds.Split(',').Select(p => p.Trim()).ToArray();
                    dest.CreatedAt = src.CreateAt;
                    
                });

            CreateMap<Stage, StageForGetDTO>()
                .AfterMap((src, dest) =>
                {
                    dest.Action     = src.Action.GetDescription();
                    dest.GroupClass = string.IsNullOrEmpty(src.GroupIds) ? null : src.GroupClass.GetDescription();
                    dest.UserType   = src.UserType.GetDescription();
                    dest.AssigneeIds = string.IsNullOrEmpty(src.AssigneeIds)
                        ? new string[] { }
                        : src.AssigneeIds.Split(',').Select(p => p.Trim()).ToArray();
                    dest.GroupIds = string.IsNullOrEmpty(src.GroupIds)
                        ? new string[] { }
                        : src.GroupIds.Split(',').Select(p => p.Trim()).ToArray();
                    dest.CreatedAt = src.CreateAt;

                });

            CreateMap<StageForCreationDTO, Stage>()
                .ForMember(
                    dest => dest.Action,
                    opt => opt.MapFrom(src => (EAction)src.Action.ParseStringToEnum(typeof(EAction))))
                .ForMember(
                           dest => dest.GroupClass,
                           opt =>
                               opt.MapFrom(src => src.GroupClass == null ? (Enum)null : (EGroupClass) src.GroupClass.ParseStringToEnum(typeof(EGroupClass))))
                .ForMember(
                           dest => dest.UserType,
                           opt => opt.MapFrom(src => (EUserType)src.UserType.ParseStringToEnum(typeof(EUserType))))
                .ForMember(
                    dest => dest.AssigneeIds,
                    opt => opt.MapFrom(src => src.AssigneeIds == null ? null : string.Join(",", src.AssigneeIds.ToArray())))
                .ForMember(
                    dest => dest.GroupIds,
                    opt => opt.MapFrom(src => src.GroupIds == null ? null : string.Join(",", src.GroupIds.ToArray())));

            CreateMap<StageForUpdateDTO, Stage>()
                .ForMember(
                    dest => dest.Action,
                    opt => opt.MapFrom(src => (EAction)src.Action.ParseStringToEnum(typeof(EAction))))
                .ForMember(
                           dest => dest.GroupClass,
                           opt =>
                               opt.MapFrom(src => src.GroupClass == null ? (Enum)null : (EGroupClass)src.GroupClass.ParseStringToEnum(typeof(EGroupClass))))
                .ForMember(
                           dest => dest.UserType,
                           opt => opt.MapFrom(src => (EUserType)src.UserType.ParseStringToEnum(typeof(EUserType))))
                .ForMember(
                    dest => dest.AssigneeIds,
                    opt => opt.MapFrom(src => src.AssigneeIds == null ? null : string.Join(",", src.AssigneeIds.ToArray())))
                .ForMember(
                    dest => dest.GroupIds,
                    opt => opt.MapFrom(src => src.GroupIds == null ? null : string.Join(",", src.GroupIds.ToArray())));



            CreateMap<StageForUnderWorkflowUpdateDTO, Stage>()
                .ForMember(
                    dest => dest.Action,
                    opt => opt.MapFrom(src => (EAction)src.Action.ParseStringToEnum(typeof(EAction))))
                .ForMember(
                           dest => dest.GroupClass,
                           opt =>
                               opt.MapFrom(src => src.GroupClass == null ? (Enum)null : (EGroupClass)src.GroupClass.ParseStringToEnum(typeof(EGroupClass))))
                .ForMember(
                           dest => dest.UserType,
                           opt => opt.MapFrom(src => (EUserType)src.UserType.ParseStringToEnum(typeof(EUserType))))
                .ForMember(
                    dest => dest.AssigneeIds,
                    opt => opt.MapFrom(src => src.AssigneeIds == null ? null : string.Join(",", src.AssigneeIds.ToArray())))
                .ForMember(
                    dest => dest.GroupIds,
                    opt => opt.MapFrom(src => src.GroupIds == null ? null : string.Join(",", src.GroupIds.ToArray())));
                    
        }
    }
}
