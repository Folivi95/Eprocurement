using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IProcurementPlanRepository : IRepository<ProcurementPlan>
    {
        Task<IEnumerable<ProcurementPlanType>> GetProcurementPlanTypes(string type);
        Task AddProcurementPlanDocument(IEnumerable<ProcurementPlanDocument> documents);
        Task AddNoticeInformation(NoticeInformation noticeInformation);
        Task AddProcurementPlanReviewDocument(ProcurementPlanDocument document);
        Task<bool> IsProcurementPlanActivityExist(Guid procurementPlanActivityId);

        Task<ProcurementPlanActivity> GetProcurementPlanActivity(Guid procurementPlanActivityId);
        Task<bool> IsProcurementPlanDocumentExist(Guid procurementPlanDocumentId);

        Task AddProcurementPlanReview(Review review);
        Task<ProcurementPlan> GetProcurementPlan(Guid procurementPlanId);
        void UpdateProcurementPlanActivity(ProcurementPlanActivity procurementPlanActivity);
        Task<IEnumerable<ProcurementPlanDocument>> GetProcurementPlanDocument(Guid procurementPlanActivityId);

        Task<IEnumerable<ProcurementPlanDocument>> GetProcurementPlanDocumentWithObjectType(
            Guid procurementPlanActivityId, EDocumentObjectType? objectType);

        Task<IEnumerable<Review>> GetProcurementPlanActivityReview(Guid procurementPlanActivityId);
        Task AddDatasheet(Datasheet datasheet);
        void UpdateDatasheet(Datasheet datasheet);
        Task<Datasheet> GetProcurementDatasheet(Guid procurementPlanActivityId);
        Task<bool> IsIndicatedVendor(Guid vendorId, Guid procurementPlanId);
        Task AddIndicatedVendor(VendorProcurement vendorProcurement);
        Task RemoveIndicatedVendor(ICollection<Guid?> vendorId);
        Task<List<Guid>> GetIndicatedVendors(Guid procurementPlanId);
        Task<bool> IsProcurementPlanExists(Guid procurementPlanId);
        Task AddProcurementPlanActivity(List<ProcurementPlanActivity> activities);
        Task<Review> GetProcurementReviewById(Guid reviewId);

        void UpdateVendorProcurements(VendorProcurement vendorProcurement);
        Task<PagedList<ContractsDTO>> GetContracts(Guid procurementPlanId, ContractsParameters parameters);

        Task<ProcurementMethod> GetProcurementMethod(Guid procurementMethodId);
        Task<QualificationMethod> GetQualificationMethod(Guid qualificationMethodId);
        Task<ProcurementCategory> GetProcurementCategory(Guid procurementCategoryId);
        Task<ReviewMethod> GetReviewMethod(Guid reviewMethodId);
        Task<ProcurementProcess> GetProcurementProcess(Guid procurementProcessId);

        Task<ProcurementPlanNumber> GetProcurementPlanNumber(ProcurementPlanNumberParameters parameters);
        Task<IEnumerable<ProcurementCategory>> GetAllProcurementCategories();
        Task<IEnumerable<ProcurementMethod>> GetAllProcurementMethods();
        Task<IEnumerable<ProcurementProcess>> GetAllProcurementProcesses();
        Task<IEnumerable<QualificationMethod>> GetAllQualificationMethods();
        Task<IEnumerable<ReviewMethod>> GetAllReviewMethods();

        void UpdateNoticeInformation(NoticeInformation noticeInformation);
        Task<NoticeInformation> GetNoticeInformation(Guid noticeInformationId);
        Task<NoticeInformation> GetNoticeInformationForProcurementPlan(Guid procurementPlanId);

        Task RemoveProcurementPlanDocument(IEnumerable<Guid?> documentId);

        Task<bool> IsProcurementPlanActivityDocumentExists(Guid procurementPlanActivityId);

        void UpdateProcurementPlanActivityRevisedDate(Guid procurementPlanActivityId);
        void UpdateVendorBids(VendorBid vendorBid);
        Task<List<Guid>> GetBidVendors(Guid procurementPlanId);
        Task<PagedList<ProcurementPlanActivity>> GetProcurementPlanActivityByTitle(string title, Guid procurementPlanId, ResourceParameters parameters);
        Task<PagedList<ProcurementPlanActivity>> GetProcurementPlanActivityByBidTitle(string title, BidsParameters parameters);
        Task<ProcurementPlanActivity> GetVendorPlanActivityByBidTitle(string title, Guid ProcurementPlanId);
        Task<DateTime> GetBidExpiryDate(Guid ProcurementPlanActivityId);
        Task<PagedList<VendorBid>> GetBiddersForProcurementPlan(Guid userId, ResourceParameters parameters);
        Task<PagedList<NoticeInformation>> GetNoticeInformation(SpecialNoticeInformationParameters parameters);

        Task<IEnumerable<ProcurementPlanActivity>> GetProcurementPlanActivities(Guid procurementPlanId);
        Task<ProcurementPlanDocument> GetProcurementPlanActivityDocument(Guid procurementPlanActivityId);
        Task<IEnumerable<VendorBid>> GetProccessingBidVendors(ICollection<Guid?> vendorId);
        Task<bool> IsBidVendor(Guid vendorId, Guid procuremntPlanId);
        Task AddBidVendor(VendorBid vendorBid);
        Task<Contract> GetContract(Guid procurementPlanId);
        Task<bool> IsProcurementPlanActivityTitleExists(string procurementPlanActivityTitle);

        Task<PagedList<VendorBid>> GetBidsForVendor(Guid? userId, BidsTableParameters parameters);
        Task<IEnumerable<VendorBid>> GetInterestedVendorBids(Guid procurementPlanId);
        Task<DateTime> GetNoticeExpiryDate(Guid procurementPlanId);
        Task<ProcurementSummaryDTO> GetProcurementSummary();
        Task<ProcurementSummaryDTO> GetProcurementSummaryByMinistry(Guid ministryId);
        Task<PagedList<ProcurementTender>> GetTenderProcurments(ProcurementTenderParameters parameters);
        Task<ProcurementTender> GetTender(Guid tenderId);
        Task<Contract> GetAcceptedContract(Guid procurementPlanId);
    }
}
