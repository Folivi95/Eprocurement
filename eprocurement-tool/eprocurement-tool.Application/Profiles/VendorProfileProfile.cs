using System;
using AutoMapper;
using EGPS.Application.Helpers;
using EGPS.Application.Models;
using EGPS.Domain.Entities;

namespace EGPS.Application.Profiles
{
    public class VendorProfileProfile : Profile
    {
        public VendorProfileProfile()
        {
            CreateMap<User, VendorProfileUserDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
            });

            CreateMap<VendorContact, VendorContactDTO>().ReverseMap();
            CreateMap<VendorCorrespondence, VendorCorrespondenceDTO>().ReverseMap();

            CreateMap<VendorProfile, VendorProfileDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
                dest.Status = src.Status.GetDescription();
            }).ReverseMap();


            CreateMap<VendorProfile, VendorProfileForContractDTO>()
                .ForMember(dest => dest.CreatedAt, src => src.MapFrom(s => s.CreateAt)).ReverseMap();
            CreateMap<VendorProfile, BidEvaluationVendorDto>().ReverseMap();
            CreateMap<BusinessService, BusinessServiceDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
            }); ;
            CreateMap<VendorAttestation, VendorAttestationDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
            }); ;
            CreateMap<VendorProfileForCreationDTO, VendorProfile>();
            CreateMap<VendorProfileForUpdateDTO, VendorProfile>();
            CreateMap<VendorProfileForUpdateDTO, User>();
            CreateMap<VendorCorrespondenceForCreationDTO, VendorCorrespondence>();
            CreateMap<VendorDocument, VendorDocumentDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
            }); ;
            CreateMap<VendorDirector, VendorDirectorDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
            }); ;

            CreateMap<VendorDirectorForUpdateDTO, VendorDirector>().AfterMap((src, dest) =>
            {
                dest.UpdatedAt = DateTime.Now;
            }).ReverseMap();

            CreateMap<VendorDirectorCertificate, VendorDirectorCertificateDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
            });

            CreateMap<VendorDirectorForCreationDTO, VendorDirector>();
            CreateMap<VendorDirectorCertificateForCreationDTO, VendorDirectorCertificate>();

            CreateMap<VendorDocumentType, VendorDocumentTypeDTO>()
                .ForMember(dest => dest.CreatedAt, src => src.MapFrom(s => s.CreateAt));

            CreateMap<BusinessService, BusinessServiceDTO>()
                .ForMember(dest => dest.BusinessCategory, src => src.MapFrom(s => s.BusinessCategory)).AfterMap((src, dest) =>
                {
                    dest.CreatedAt = src.CreateAt;
                }).ReverseMap();

            CreateMap<BusinessCategory, BusinessCategoryDTO>()
                .ForMember(dest => dest.CreatedAt, src => src.MapFrom(s => s.CreateAt));

            CreateMap<RegistrationPlan, RegistrationPlanDTO>().ForMember(dest => dest.CreatedAt, src => src.MapFrom(s => s.CreateAt));
            CreateMap<VendorRegistrationCategory, VendorRegistrationCategoryDTO>()
                .ForMember(dest => dest.CreatedAt, src => src.MapFrom(s => s.CreateAt))
                .ForMember(dest => dest.RegistrationPlan, src => src.MapFrom(s => s.RegistrationPlan));

            CreateMap<User, UserVendorDTO>()
                .ForMember(dest => dest.CreatedAt, src => src.MapFrom(s => s.CreateAt));
            CreateMap<VendorProfile, EvaluatedBidResponse>().ReverseMap();
        }
    }
}
