using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGPS.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EGPS.Application.Models
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => (CurrentPage > 1);
        public bool HasNext => (CurrentPage < TotalPages);

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static async Task<PagedList<T>> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            if (pageSize == 0)
            {
                pageSize = count;
            }
            var items = await source.Skip(((pageNumber - 1) * pageSize)).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }



    }

    public enum ResourceUriType
    {
        PreviousPage,
        NextPage,
        CurrentPage
    }

    public class UserPageModel : ResourceParameters
    {
        public string Search { get; set; }
        public Guid[] userIds { get; set; }
    }

    public class DocumentClassParameters : ResourceParameters
    {
    }

    public class DepartmentParameters : ResourceParameters
    {
        public string Search { get; set; }
    }

    public class StaffParameters : ResourceParameters
    {
        public string name { get; set; }
        public Guid? ministryId { get; set; }
        public Guid? roleId { get; set; }
        public EStatus? status { get; set; }
    }

    public class MinistryParameters : ResourceParameters
    {
        public string name { get; set; }
        public string code { get; set; }
        public Guid? estimatedValueId { get; set; }
        public Guid? bidLowerThanId { get; set; }
        public double? estimatedValue { get; set; }
    }

    public class TransactionParameters : ResourceParameters
    {
        public string Search { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public EInvoiceStatus Status { get; set; } = EInvoiceStatus.PAID;
    }

    public class NotificationParameters : ResourceParameters
    {
        public string Search { get; set; }
        public List<ENotificationClass> Class { get; set; } = new List<ENotificationClass>();
        public DateTime? ExpiryDate { get; set; }
        public bool? Read { get; set; }

    }

    public class UnitParameters : ResourceParameters
    {
        public string Search { get; set; }
        public Guid? DepartmentId { get; set; }
    }

    public class RoleParamaters : ResourceParameters
    {
        public string Type { get; set; }
    }

    public class DepartmentMembersParameter : ResourceParameters
    {
        public string Search { get; set; }
    }

    public class BusinessServicesParameter : ResourceParameters
    {
    }

    public class StateParameter : ResourceParameters
    {
    }

    public class CountryParameter : ResourceParameters
    {
    }

    public class VendorBidParameter : ResourceParameters
    {

    }

    public class RegistrationPlanParameter : ResourceParameters
    {
    }

    public class RegistrationCategoryParameter : ResourceParameters
    {
    }

    public class VendorAttestationParameter : ResourceParameters
    {
    }

    public class VendorDocumentParameter : ResourceParameters
    {
    }

    public class VendorDocumentTypeParameter : ResourceParameters
    {
    }

    public class VendorDirectorParameter : ResourceParameters
    {
    }

    public class UnitMembersParameter : ResourceParameters
    {
        public string Search { get; set; }
    }

    public class StageParameter : ResourceParameters
    {
    }

    public class WorkflowParameter : ResourceParameters
    {
        public string Search { get; set; }
    }

    public class AuditParameters : ResourceParameters
    {
        public string Search { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class Commentparameters : ResourceParameters
    {
        public string Type { get; set; }
        public Guid? ObjectId { get; set; }
    }

    public class GetAllVendorParameters : ResourceParameters
    {
        public string RegisterPlanTitle { get; set; }
        public string CompanyName { get; set; }
        public EVendorStatus Status { get; set; }
    }

    public class VendorParameters : ResourceParameters
    {
        public string RegisterId { get; set; }
        public string Name { get; set; }
        public EVendorStatus Status { get; set; }
    }

    public class ProcurementPlanNumberParameters
    {
        public string Ministry { get; set; }
        public string ProcurementCategory { get; set; }
        public string ProcurementMethod { get; set; }
    }

    public class GeneralPlanParameters : ResourceParameters
    {
        public string Search { get; set; }
        public Guid? MinistryId { get; set; }
        public string Status { get; set; }
        public int? Year { get; set; }
    }

    public class ProcurementPlanParameters : ResourceParameters
    {
        public string Search { get; set; }
        public Guid? MinistryId { get; set; }
        public string Status { get; set; }
        public string Stage { get; set; }
        public string Category { get; set; }
    }
    public class BidsTableParameters : ResourceParameters
    {
        public string Search { get; set; }
        public string Process { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public EVendorContractStatus? Type { get; set; }

    }
    public class ResourceParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class SpecialNoticeInformationParameters : ResourceParameters
    {
        public string Title { get; set; }
        public string ProcurementCategory { get; set; }
        public string Ministry { get; set; }
        public DateTime? ExpiryDate { get; set; }

    }

    public class BidsParameters : ResourceParameters
    {
        public string Title { get; set; }
        public string Process { get; set; }

        public EBidStatus Status { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public class ProjectParameters : ResourceParameters
    {
        public string Title { get; set; }
        public string Category { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public class ContractParameters : ResourceParameters
    {
        public string SearchBy { get; set; }
        public string Search { get; set; }
        public string Status { get; set; }
        public DateTime? Date { get; set; }
    }

    public class MileStoneTaskParameter : ResourceParameters
    {

    }

    public class InitiatePaymentParameter
    {
        public string CallbackUrl { get; set; }
    }

    public class ProcurementContractParameters : ResourceParameters
    {
        public string Search { get; set; }
        public string Category { get; set; }
        public DateTime? DateAwarded { get; set; }
    }

    public class ProcurementTenderParameters : ResourceParameters
    {
        public Guid? MinistryId { get; set; }
        public string Search { get; set; }
        public string Category { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public class ContractsParameters : ResourceParameters
    {

        public ESignatureStatus? SignatureStatus { get; set; }
        public EContractStatus? Status { get; set; }

    }
}
