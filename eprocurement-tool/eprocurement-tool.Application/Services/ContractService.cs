using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EGPS.Application.Interfaces;
using EGPS.Domain.Enums;
using Microsoft.Extensions.Hosting;

namespace EGPS.Application.Services
{
    public class ContractService : IContractService
    {

        public DateTime ConvertDuration(int duration, EDurationType type)
        {
            if (type == EDurationType.MONTH)
            {
                var date = DateTime.Now.AddMonths(duration);
                return date;
            }

            if (type == EDurationType.YEAR)
            {
                var date = DateTime.Now.AddYears(duration);
                return date;
            }
            if (type == EDurationType.WEEK)
            {
                var date = DateTime.Now.AddDays(duration * 7);
                return date;
            }
            if (type == EDurationType.DAY)
            {
                var date = DateTime.Now.AddDays(duration);
                return date;
            }

            return DateTime.Today;
        }
    }
}
