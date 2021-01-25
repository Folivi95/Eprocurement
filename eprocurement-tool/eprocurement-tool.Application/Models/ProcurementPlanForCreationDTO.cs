using EGPS.Application.Common;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class ProcurementPlanForCreationDTO
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public Guid ProcurementCategoryId { get; set; }
        public Guid ProcessTypeId { get; set; }
        public Guid ProcurementMethodId { get; set; }
        public double EstimatedAmountInNaira { get; set; }
        public double EstimatedAmountInDollars { get; set; }
        public Guid QualificationMethodId { get; set; }
        public Guid ReviewMethodId { get; set; }
        public Guid MinistryId { get; set; }
        public string Description { get; set; }
        public string PackageNumber { get; set; }
        public Guid? GeneralPlanId { get; set; }
    }

    public class ProcurementPlanDocumentCreation
    {
        public ICollection<Guid?> RemovedDocuments { get; set; }
        public List<IFormFile> MandatoryDocument { get; set; } = new List<IFormFile>();
        public List<IFormFile> SupportingDocument { get; set; } = new List<IFormFile>();
        public EDocumentObjectType? ObjectType { get; set; }
    }

    public class GenericProcurementPlanDocumentDto
    {
        public Guid UserId { get; set; }
        public List<IFormFile> Documents { get; set; }
        public EDocumentStatus Status { get; set; }
        public Guid ObjectId { get; set; }
        public EDocumentObjectType ObjectType { get; set; }
    }

    public class ProcurementPlanDocumentDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ProcurementDocumentStatus { get; set; }
        public Guid CreatedById { get; set; }
        public Guid? ObjectId { get; set; }
        public string ObjectType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public object File { get; set; }
    }

    public class ContractAwardDocumentCreation
    {
        public ProcurementPlanDocumentCreation Documents { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class ContractAwardDocumentDTO : AuditableModelDTO
    {
        public IEnumerable<ProcurementPlanDocumentDTO> Documents { get; set; }
        public ContractAwardDatasheet Datasheet { get; set; }
    }

    public class DocumentDatasheetCreation
    {
        public ProcurementPlanDocumentCreation Documents { get; set; }
        public DateTime SubmissionDeadline { get; set; }
    }

    public class DocumentDatasheetDTO
    {
        public IEnumerable<ProcurementPlanDocumentDTO> Documents { get; set; }
        public DocumentDatasheet Datasheet { get; set; }
    }

    public class ContractSigningDocumentAndDatasheetDTO
    {
        public IEnumerable<ProcurementPlanDocumentDTO> Documents { get; set; }
        public ContractSigningDatasheet Datasheet { get; set; }
    }

    public class ContractSigningDocumentAndDatasheetCreation
    {
        public ProcurementPlanDocumentCreation Documents { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public DateTime SignatureDate { get; set; }
        public DateTime SubmissionDeadline { get; set; }
    }

    public class BidEvaluationForCreation
    {
        public ICollection<Guid?> AddedVendors { get; set; }
        public ICollection<Guid?> RemovedVendors { get; set; }
    }

    public class BidEvaluationResponse
    {
        public List<BidEvaluationVendorDto> Vendors { get; set; }

    }

    public class ProcurementPlanForUpdateDTO
    {
        public string Name { get; set; }
        public Guid ProcurementCategoryId { get; set; }
        public Guid ProcessTypeId { get; set; }
        public Guid ProcurementMethodId { get; set; }
        public double EstimatedAmountInNaira { get; set; }
        public double EstimatedAmountInDollars { get; set; }
        public Guid QualificationMethodId { get; set; }
        public Guid ReviewMethodId { get; set; }
        public Guid MinistryId { get; set; }
        public string Description { get; set; }
        public string PackageNumber { get; set; }
        public Guid GeneralPlanId { get; set; }
    }
}
