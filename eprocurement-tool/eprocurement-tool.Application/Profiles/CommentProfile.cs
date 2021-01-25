using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EGPS.Application.Helpers;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using Newtonsoft.Json;

namespace EGPS.Application.Profiles
{
    class CommentProfile: Profile
    {
        public CommentProfile()
        {
            CreateMap<CommentForCreation, Comment>().ForMember(
                    dest => dest.Type,
                    opt => opt.MapFrom(src => (CommentType)src.Type.ParseStringToEnum(typeof(CommentType))));

            CreateMap<Comment, CommentDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
                dest.Type = src.Type.GetDescription();
            });
        }
    }
}
