using AutoMapper;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Profiles
{
    public class ContractsProfile : Profile
    {
        public ContractsProfile()
        {
            CreateMap<Contract, ContractsDTO>()
                .ForMember(dest => dest.EstimatedValue, src => src.MapFrom(s => s.EstimatedValue))
                .ForMember(dest => dest.CreatedById, src => src.MapFrom(c => c.UserId))
                .ForMember(dest => dest.CreatedAt, src => src.MapFrom(s => s.CreateAt))
                .ForMember(dest => dest.Category, src => src.MapFrom(c => c.RegistrationPlan.Title))
                .ForMember(dest => dest.PercentageCompleted, src => src.MapFrom(c => c.PercentageCompletion))
                .ForMember(d => d.Contractor, s => s.MapFrom(c => c.Contractor.VendorProfile));

            CreateMap<Contract, CreatedContractDTO>()
                .ForMember(dest => dest.EstimatedValue, src => src.MapFrom(s => s.EstimatedValue))
                .ForMember(dest => dest.CreatedById, src => src.MapFrom(c => c.UserId))
                .ForMember(dest => dest.CreatedAt, src => src.MapFrom(s => s.CreateAt))
                .ForMember(dest => dest.Category, src => src.MapFrom(c => c.RegistrationPlan.Title))
                .ForMember(dest => dest.PercentageCompleted, src => src.MapFrom(c => c.PercentageCompletion));

            CreateMap<Contract, ContractAwardDTO>()
                .ForMember(dest => dest.EstimatedValue, src => src.MapFrom(s => s.EstimatedValue))
                .ForMember(dest => dest.CreatedById, src => src.MapFrom(c => c.UserId))
                .ForMember(dest => dest.CreatedAt, src => src.MapFrom(s => s.CreateAt))
                .ForMember(dest => dest.PercentageCompletion, src => src.MapFrom(c => c.PercentageCompletion));


            CreateMap<Contract, ContractsForVendorDTO>()
                .ForMember(dest => dest.CreatedById, src => src.MapFrom(c => c.UserId))
                .ForMember(dest => dest.CreatedAt, src => src.MapFrom(s => s.CreateAt))
                .ForMember(dest => dest.ContractStatus, src => src.MapFrom(s => s.Status));

            CreateMap<VendorProfile, VendorProfileForContractDTO>();
        }
    }
}
