using EGPS.Application.Interfaces;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Repository
{
    public class BusinessCategoryRepository:Repository<BusinessCategory>, IBusinessCategoryRepository
    {
        public BusinessCategoryRepository(EDMSDBContext context):
            base(context)
        {

        }
    }
}
