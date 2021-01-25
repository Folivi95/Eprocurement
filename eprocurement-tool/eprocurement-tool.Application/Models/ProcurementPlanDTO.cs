using EGPS.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EGPS.Application.Models
{
    public class ProcurementPlanForDeletedDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public Guid BusinessCategoryId { get; set; }
        public Guid ProcessTypeId { get; set; }
        public Guid MethodId { get; set; }
        public double EstimatedAmountInNaira { get; set; }
        public double EstimatedAmountInDollars { get; set; }
        public string QualificationMethod { get; set; }
        public Guid ReviewMethodId { get; set; }
        public Guid MinistryId { get; set; }
        public string Description { get; set; }
        public string PackageNumber { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletedAt { get; set; }
    }


    public class ProcurementPlanTypeDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProcurementPlanTask { get; set; }
    }

    public class ProcurementPlanDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ProcurementCategoryId { get; set; }
        public ProcurementCategory ProcurementCategory { get; set; }
        public Guid ProcessTypeId { get; set; }
        public ProcurementProcess ProcurementProcess { get; set; }
        public Guid ProcurementMethodId { get; set; }
        public double EstimatedAmountInNaira { get; set; }
        public double EstimatedAmountInDollars { get; set; }
        public Guid QualificationMethodId { get; set; }
        public Guid ReviewMethodId { get; set; }
        public Guid MinistryId { get; set; }
        public MinistryDTO Ministry { get; set; }
        public string Description { get; set; }
        public string PackageNumber { get; set; }
        public Guid GeneralPlanId { get; set; }
        public Guid CreatedById { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EProcurementStage Stage { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EProcurementPlanStatus Status { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EProcurementSectionStatus SectionOne { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EProcurementSectionStatus SectionTwo { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EProcurementSectionStatus SectionThree { get; set; }
    }

    public class ProcurementDocumentDTO
    {
        public IEnumerable<ProcurementPlanDocument> Documents { get; set; }
        public Datasheet DatasheetDocument { get; set; }
    }


    public class ContractAwardDocumentResponse
    {
        public ContractAwardDatasheet Datasheet { get; set; }
        public IEnumerable<ProcurementPlanDocumentDTO> Documents { get; set; }
    }

    public class ContractAwardDatasheet
    {
        public Guid Id { get; set; }
        public DateTime? IssuedDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class DocumentDatasheetResponse
    {
        public DocumentDatasheet Datasheet { get; set; }
        public IEnumerable<ProcurementPlanDocumentDTO> Documents { get; set; }
    }

    public class DocumentDatasheet
    {
        public Guid Id { get; set; }
        public DateTime? SubmissionDeadline { get; set; }
    }

    public class ContractSigningDocumentResponse
    {
        public ContractSigningDatasheet Datasheet { get; set; }
        public IEnumerable<ProcurementPlanDocumentDTO> Documents { get; set; }
    }

    public class ContractSigningDatasheet
    {
        public Guid Id { get; set; }
        public DateTime? SignatureDate { get; set; }
        public DateTime? SubmissionDeadline { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
    }

    public class ProcurementActivityForCreation
    {
        public ActivityForCreation[] Activities { get; set; }
    }
    public class ActivityForCreation
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string ProcurementPlanType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Index { get; set; }
    }

    public class ActivityForEdit
    {

        public string Title { get; set; }
        public EPprocurementPlanTask? ProcurementPlanType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? Index { get; set; }
    }

    public class ProcurementActivityDTO : AuditableModelDTO

    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProcurementPlanType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Index { get; set; }
        public string ProcurementPlanActivityStatus { get; set; }
        public Guid ProcurementPlanId { get; set; }
        public Guid CreatedById { get; set; }
        public DateTime? RevisedDate { get; set; }
        public DateTime? ActualDate { get; set; }

    }

    public class ProcurementPlanNumberDTO : AuditableModelDTO
    {
        public string ProcurementPlanNumber { get; set; }
    }

    public class ProcurementsResponse: AuditableModelDTO
    {
        public Guid Id { get; set; }
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

        [JsonConverter(typeof(StringEnumConverter))]
        public EProcurementStage Stage { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EProcurementPlanStatus Status { get; set; }
        public ProcurmentCategoryDTO Category { get; set; }
        public ProcurmentMethodDTO Method { get; set; }
    }

    public class ProcurmentCategoryDTO
    {
        public string Name { get; set; }
    }

    public class ProcurementMinistryDTO
    {
        public string Name { get; set; }
    }
    public class BidsTable
    {
        public Guid Id { get; set; }
        public string Title { get; set; } 
        public string Description { get; set; }
        public string Process { get; set; }
        public string BidStatus { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal? Value { get; set; }
        public string Ministry { get; set; }
        public Guid ProcurementId { get; set; }
        public string PackageNumber { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }


    }
    public class ProcurmentMethodDTO
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class ProcurementPlanActivityDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ProcurementPlanId { get; set; }
        public ProcurementPlanDTO ProcurementPlan { get; set; }
        public ICollection<ProcurementPlanDocumentDTO> ProcurementPlanDocuments { get; set; }
        public ICollection<ReviewResponse> Reviews { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EPprocurementPlanTask ProcurementPlanType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EProcurementPlanActivityStatus ProcurementPlanActivityStatus { get; set; } = EProcurementPlanActivityStatus.INACTIVE;

        public Guid CreatedById { get; set; }

        public DateTime? RevisedDate { get; set; }
        public DateTime? ActualDate { get; set; }
    }

    public class ProcurementProcessDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ProcurementPlanForNoticeInformationDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ProcurementCategoryId { get; set; }
        public ProcurementCategory ProcurementCategory { get; set; }
        public Guid ProcessTypeId { get; set; }
        public ProcurementProcess ProcurementProcess { get; set; }
        public double EstimatedAmountInNaira { get; set; }
        public double EstimatedAmountInDollars { get; set; }
        public Guid MinistryId { get; set; }
        public MinistryDTO Ministry { get; set; }
        public string Description { get; set; }
        public string PackageNumber { get; set; }
        public Guid GeneralPlanId { get; set; }
    }
    public class ProcurementSummaryDTO
    {
        public int Total { get; set; }
        public int Approved { get; set; }
        public int InReview { get; set; }
        public int Draft { get; set; }
    }
    public class ProcurementResponse : ProcurementPlanDTO
    {
        public ProcurmentCategoryDTO Category { get; set; }
        public ProcurementContract Contract { get; set; }
    }

    public class ProcurementTender
    {
        public Guid TenderId { get; set; }
        public Guid ProcurementPlanId { get; set; }
        public string PackageNumber { get; set; }
        public string Name { get; set; }
        public Guid ProcurementCategoryId { get; set; }
        public double EstimatedValueInNaria { get; set; }
        public string Description { get; set; }
        public ProcurmentCategoryDTO Category { get; set; }
        public Guid MinistryId { get; set; }
        public ProcurementMinistryDTO Ministry { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
    }
}
