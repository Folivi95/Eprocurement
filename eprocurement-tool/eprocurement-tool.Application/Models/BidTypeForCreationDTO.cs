using System;

namespace EGPS.Application.Models
{
    public class BidTypeForCreationDTO
    {
        public Guid Title { get; set; }
        public string Description { get; set; }
        public Guid CreatedById { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}
