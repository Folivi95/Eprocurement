using EGPS.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class NoticeInformationForCreation
    {
        public Guid? Id { get; set; }
        public DateTime SubmissionDeadline { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Organization { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
    }

    public class NoticeInformationForUpdate
    {
        public DateTime SubmissionDeadline { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Organization { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
    }
    public class NoticeInformationResponse : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public DateTime SubmissionDeadline { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Organization { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public Guid CreatedById { get; set; }
        public Guid ProcurementPlanId { get; set; }
        public ProcurementPlanForNoticeInformationDTO ProcurementPlan { get; set; }
        public IEnumerable<ProcurementPlanDocumentDTO> Documents { get; set; }
    }

    public class NoticeInformationDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public DateTime SubmissionDeadline { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public Guid ProcurementPlanId { get; set; }
        public ProcurementPlanDTO ProcurementPlan { get; set; }
        public string Organization { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public Guid CreatedById { get; set; }
    }
}
