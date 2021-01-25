using System;
using EGPS.Application.Common;
using System.Collections.Generic;
using System.Text;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EGPS.Application.Models
{
    public class ContractsDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public Guid ContractorId { get; set; }

        public VendorProfileForContractDTO Contractor { get; set; }
        public string ContractNumber { get; set; }
        public string EvaluationCurrency { get; set; }
        public string ReferenceId { get; set; }
        public string Description { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EContractStatus Status { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ESignatureStatus SignatureStatus { get; set; }

        public Guid? RegistrationPlanId { get; set; }

        public RegistrationPlanDTO RegistrationPlan { get; set; }
        public double? EstimatedValue { get; set; }
        public DateTime? SignedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Duration { get; set; }
        public EDurationType Type { get; set; }
        public Guid CreatedById { get; set; }
        public double PercentageCompleted { get; set; }
        public bool IsUploaded { get; set; }
        public ProcurementPlanForContractDto ProcurementPlan { get; set; }
    }


    public class IndividualContractDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public VendorProfileForContractDTO Contractor { get; set; }
        public string ContractNumber { get; set; }
        public string EvaluationCurrency { get; set; }
        public string ReferenceId { get; set; }
        public string Description { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EContractStatus Status { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ESignatureStatus SignatureStatus { get; set; }

        public double? EstimatedValue { get; set; }
        public DateTime? SignedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Duration { get; set; }
        public EDurationType Type { get; set; }
        public Guid CreatedById { get; set; }
        public double PercentageCompleted { get; set; }
        public bool IsUploaded { get; set; }
        public ProcurementPlanForContractDto ProcurementPlan { get; set; }
        public IEnumerable<ProcurementPlanDocumentDTO> Documents { get; set; }
    }

    public class ContractsSummaryDTO
    {
        public int Total { get; set; }
        public int Awarded { get; set; }
        public int Signed { get; set; }
        public int Unsigned { get; set; }
        public int Pending { get; set; }
        public int Rejected { get; set; }
    }

    public class ContractForCreation
    {
        public string Description { get; set; }
        public string EvaluationCurrency { get; set; }
        public int Duration { get; set; }
        public string Type { get; set; }
        //public VendorBidForCreationDTO VendorBid { get; set; }
    }

    public class ProcurementPlanForContractDto : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ProcurementMethod ProcurementMethod { get; set; }
        public double EstimatedAmountInNaira { get; set; }
        public double EstimatedAmountInDollars { get; set; }
        //  public ReviewMethod ReviewMethod { get; set; }
        public string Ministry { get; set; }
        public string MinistryCode { get; set; }
        public string Description { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EProcurementPlanStatus Status { get; set; }
        public string PackageNumber { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }


    public class VendorProfileForContractDTO : AuditableModelDTO
    {
        public Guid UserId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string CACRegistrationNumber { get; set; }
        //public string AddressLine1 { get; set; }
        //  public string AddressLine2 { get; set; }
        //  public string Website { get; set; }
        //public string City { get; set; }
        // public string State { get; set; }
        //public string Country { get; set; }
        //  public EVendorStatus Status { get; set; }
        //  public ECoreCompetencies CoreCompetency { get; set; }
        //  public EOrganizationTypes OrganizationType { get; set; }
        //  public VendorCorrespondenceDTO VendorCorrespondence { get; set; }
        //  public VendorContactDTO VendorContact { get; set; }
        //  public DateTime IncorporationDate { get; set; }
        //  public string CACRegistrationNumber { get; set; }
        //  public string AuthorizedShareCapital { get; set; }
        // public Guid RegistrationPlanId { get; set; }
        // public bool IsRegistrationComplete { get; set; }
        // public RegistrationPlanDTO RegistrationPlan { get; set; }
    }

    public class ContractsForVendorDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid ContractorId { get; set; }
        public string ContractNumber { get; set; }
        public string EvaluationCurrency { get; set; }
        public Guid UserId { get; set; }
        public decimal? EstimatedValue { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ESignatureStatus SignatureStatus { get; set; }
        public Guid RegistrationPlanId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double PercentageCompletion { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EContractStatus ContractStatus { get; set; }

        public Guid ProcurementPlanId { get; set; }
        public ProcurementPlanDTO ProcurementPlan { get; set; }

        public DateTime? SignedDate { get; set; }
        public Guid CreatedById { get; set; }
        public bool IsUploaded { get; set; }

    }

    public class ContractAwardDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid ContractorId { get; set; }
        public string ContractNumber { get; set; }
        public string EvaluationCurrency { get; set; }
        public Guid UserId { get; set; }
        public decimal? EstimatedValue { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ESignatureStatus SignatureStatus { get; set; }
        public Guid RegistrationPlanId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double PercentageCompletion { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EContractStatus ContractStatus { get; set; }
        public Guid ProcurementPlanId { get; set; }
        public DateTime? SignedDate { get; set; }
        public Guid CreatedById { get; set; }
        public bool IsUploaded { get; set; }

    }
    public class ProcurementContract
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string EvaluationCurrency { get; set; }
        public double? EstimatedValue { get; set; }
        public string Vendor { get; set; }
        public DateTime DateAwarded { get; set; }
        public DateTime? ExpiryDate { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EContractStatus Status { get; set; }
        public ContractProcurement Procurement { get; set; }
    }

    public class ContractProcurement
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ProcurementCategoryId { get; set; }
        public ProcurmentCategoryDTO Category { get; set; }
        public Guid MinistryId { get; set; }
        public ProcurementMinistryDTO Ministry { get; set; }
    }
    public class CreatedContractDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public Guid ContractorId { get; set; }
        public string ContractNumber { get; set; }
        public string EvaluationCurrency { get; set; }
        public string ReferenceId { get; set; }
        public string Description { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EContractStatus Status { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ESignatureStatus SignatureStatus { get; set; }

        public Guid? RegistrationPlanId { get; set; }

        public RegistrationPlanDTO RegistrationPlan { get; set; }
        public double? EstimatedValue { get; set; }
        public DateTime? SignedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Duration { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public EDurationType Type { get; set; }
        public Guid CreatedById { get; set; }
        public double PercentageCompleted { get; set; }
        public bool IsUploaded { get; set; }
        public ProcurementPlanForContractDto ProcurementPlan { get; set; }
    }
}
