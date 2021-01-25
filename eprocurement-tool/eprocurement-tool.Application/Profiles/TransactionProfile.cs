using AutoMapper;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<MilestoneInvoice, TransactionTableViewModel>().AfterMap((src, dest) =>
            {
                dest.Id = src.Id;
                dest.Title = src.InvoiceNumber;
                dest.Description = src.Description;
                dest.TransactionDate = src.CreateAt;
                dest.Value = (double)src.Price;
                dest.MileStoneId = src.ProjectMileStoneId;
            });
        }
    }
}
