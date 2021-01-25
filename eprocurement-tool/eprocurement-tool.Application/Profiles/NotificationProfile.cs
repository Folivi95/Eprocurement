using AutoMapper;
using EGPS.Application.Helpers;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Profiles
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDTO>()
                .ForMember(
                    dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type.GetDescription()))
                .ForMember(
                    dest => dest.NotificationType,
                    opt => opt.MapFrom(src => src.NotificationType.GetDescription()));

            CreateMap<NotificationForCreationDTO, Notification>()
                .ForMember(
                    dest => dest.NotificationType,
                    opt => opt.MapFrom(src => (ENotificationType)src.NotificationType.ParseStringToEnum(typeof(ENotificationType))))
                .ForMember(
                    dest => dest.Type,
                    opt => opt.MapFrom(src => (EType)src.Type.ParseStringToEnum(typeof(EType))));

            CreateMap<Notification, NotificationModel>().AfterMap((src, dest) =>
            {
                dest.NotificationClass = src.NotificationClass.ToString();
                dest.CreatedAt = src.CreateAt;
            });
        }
    }
}
