using AutoMapper;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Helpers;
using Newtonsoft.Json;

namespace EGPS.Application.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<Account, AccountDTO>().AfterMap((src, dest) =>
            {
                dest.CreatedAt = src.CreateAt;
                dest.CompanyLogo = string.IsNullOrEmpty(src.CompanyLogo)
                    ? null
                    : JsonConvert.DeserializeObject(src.CompanyLogo);
                dest.PhoneNumber = src.ContactPhone;
            });
            CreateMap<AccountForCreationDTO, Account>();
            CreateMap<Account, ResendAccountDTO>().AfterMap((src, dest) =>
            {
                dest.PhoneNumber = src.ContactPhone;
                dest.UserId = src.CreatedById;
            });
            CreateMap<AccountForUpdateDTO, Account>().AfterMap((src, dest) =>
            {
                dest.ContactPhone = src.PhoneNumber;
                dest.UpdatedAt = DateTime.Now;
            }); 
        }
    }
}
