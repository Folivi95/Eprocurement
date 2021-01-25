using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EGPS.Application.Models;
using EGPS.Domain.Entities;

namespace EGPS.Application.Profiles
{
    public class DocumentClassProfile: Profile
    {
        public DocumentClassProfile()
        {
            CreateMap<DocumentClass, DocumentClassDTO>()
                .AfterMap((src, dest) =>
                {
                    dest.CreatedAt = src.CreateAt;
                });
            CreateMap<DocumentClassForCreationDTO, DocumentClass>();
            CreateMap<DocumentClassForUpdateDTO, DocumentClass>();
        }
    }
}
