using AutoMapper;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EGPS.Application.Profiles
{
   public class BidTableProfile : Profile
    {
        public BidTableProfile()
        {
            CreateMap<VendorBid, BidsTable>().AfterMap((src, dest) => {
                dest.BidStatus =  src.Type?.ToString();
                dest.Id = src.Id;
                dest.Category = src.ProcurementCategory;
                dest.Description = src.ProcurementPlan?.Description ?? "";
                dest.ExpiryDate = src.ExpiryDate;
                dest.Ministry = src.Ministry;
                dest.Process = src.ProcurementType;
                dest.ProcurementId = src.ProcurementPlanId;
                dest.Value = src.Amount;
                dest.Title = src.ProcurementPlan?.Name ?? "";
                dest.Type = src.ProcurementType;
                dest.PackageNumber = src.ProcurementPlan?.PackageNumber ?? "";
            });
        }
    }
}
