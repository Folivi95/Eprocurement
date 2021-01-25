using AutoMapper;
using EGPS.Application.Helpers;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Profiles
{
    public class ProcurementPlanProfile : Profile
    {
        public ProcurementPlanProfile()
        {
            CreateMap<ProcurementPlan, ProcurementPlanDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
            });

            CreateMap<ProcurementPlan, ProcurementPlanToReturnDto>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
            });
            CreateMap<ProcurementPlan, ProcurementPlanForContractDto>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
            });

            CreateMap<ProcurementPlanType, ProcurementPlanTypeDTO>().AfterMap((src, dest) =>
            {
                dest.ProcurementPlanTask = src.ProcurementPlanTask.GetDescription();
            });

            CreateMap<ProcurementPlan, ProcurementPlanForDeletedDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
            });

            CreateMap<ProcurementPlanDocument, ProcurementPlanDocumentDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
                dest.ProcurementDocumentStatus = src.DocumentStatus.GetDescription();
                dest.ObjectType = src.ObjectType.GetDescription();
                dest.File = string.IsNullOrEmpty(src.File) ? null
                    : JsonConvert.DeserializeObject(src.File);
            });

            CreateMap<ReviewForCreation, Review>();
            CreateMap<Review, ReviewResponse>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
            });

            CreateMap<NoticeInformationForCreation, NoticeInformation>();
            CreateMap<NoticeInformation, NoticeInformationResponse>();

            CreateMap<ProcurementPlan, ProcurementPlanForDeletedDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
            });

            CreateMap<Datasheet, ContractAwardDatasheet>().AfterMap((src, dest) =>
            {
                dest.IssuedDate = src.StartDate;
                dest.EndDate = src.SubmissionDeadline;
            });

            CreateMap<Datasheet, ContractSigningDatasheet>();

            CreateMap<Datasheet, DocumentDatasheet>();

            CreateMap<ActivityForCreation, ProcurementPlanActivity>().ForMember(
                    dest => dest.ProcurementPlanType,
                    opt => opt.MapFrom(src => (EPprocurementPlanTask)src.ProcurementPlanType.ParseStringToEnum(typeof(EPprocurementPlanTask))));

            CreateMap<ProcurementPlanActivity, ProcurementActivityDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
                dest.ProcurementPlanActivityStatus = src.ProcurementPlanActivityStatus.GetDescription();
                dest.ProcurementPlanType = src.ProcurementPlanType.GetDescription();
            });

            CreateMap<User, ReviewUser>().AfterMap((src, dest) =>
            {
                dest.ProfilePicture = string.IsNullOrEmpty(src.ProfilePicture) ? null
                    : JsonConvert.DeserializeObject(src.ProfilePicture);
            });

            CreateMap<VendorProcurement, VendorProcurementDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
            });

            CreateMap<ProcurementPlanNumber, ProcurementPlanNumberDTO>()
                .ForMember(dest => dest.CreatedAt, src => src.MapFrom(s => s.CreateAt))
                .ForMember(dest => dest.ProcurementPlanNumber, src => src.MapFrom(s => s.PlanNumber));

            CreateMap<ProcurementProcess, ProcurementProcessDTO>();

            CreateMap<ProcurementPlanForCreationDTO, ProcurementPlan>();

            CreateMap<ProcurementPlan, ProcurementPlanForNoticeInformationDTO>();
            CreateMap<VendorBid, VendorBidDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
                dest.Type = src.Type.GetDescription();
            });

            CreateMap<ProcurementPlanForCreationDTO, ProcurementPlan>();

            CreateMap<ProcurementPlan, ProcurementPlanForNoticeInformationDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
            });
        }
    }
}
