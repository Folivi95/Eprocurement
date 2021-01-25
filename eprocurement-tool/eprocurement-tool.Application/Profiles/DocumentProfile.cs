using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EGPS.Application.Helpers;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using Newtonsoft.Json;

namespace EGPS.Application.Profiles
{
    public class DocumentProfile: Profile
    {
        public DocumentProfile()
        {
            CreateMap<Document, DocumentDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
                dest.DocumentStatus = src.DocumentStatus.GetDescription();
                dest.ObjectType = src.ObjectType.GetDescription();
                dest.File = string.IsNullOrEmpty(src.File) ? null : JsonConvert.DeserializeObject(src.File);
            });
            
        }
    }
}
