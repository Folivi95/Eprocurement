using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class RegistrationPlanForCreationDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Grade { get; set; }
        public decimal Fee { get; set; }
        public decimal ContractMinValue { get; set; }
        public decimal ContractMaxValue { get; set; }
        public int TenureInDays { get; set; }
    }
}
