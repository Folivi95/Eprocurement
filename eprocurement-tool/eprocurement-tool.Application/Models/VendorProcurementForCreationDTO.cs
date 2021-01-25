using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Domain.Enums;

namespace EGPS.Application.Models
{
    public class VendorProcurementForCreationDTO
    {
        public Guid VendorId { get; set; }
        public string Reason { get; set; }
        public int Position { get; set; }
        public string Currency { get; set; }
        public decimal? Amount { get; set; }
        public decimal? BidPrice { get; set; }
        public decimal? EvaluatedPrice { get; set; }
        public EVendorContractStatus Type { get; set; }
    }


    public class VendorBidForCreationDTO
    {
        public Guid VendorId { get; set; }
        public string Reason { get; set; }
        public int Position { get; set; }
        public string Currency { get; set; }
        public decimal? Amount { get; set; }
        public decimal? BidPrice { get; set; }
        public decimal? EvaluatedPrice { get; set; }
        public EVendorContractStatus Type { get; set; }
    }
}
