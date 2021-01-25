using System;

namespace EGPS.Application.Models
{
    public class TransactionProjectMilestoneDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double EstimatedValue { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProjectDetailsDTO Project { get; set; }
        public MilestoneInvoiceDTO MilestoneInvoice { get; set; }
    }


    public class TransactionTableViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public double Value { get; set; }
        public string UniqueId { get; set; } 
        public Guid MileStoneId { get; set; }
    }
}
