using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IContractRepository : IRepository<Contract>
    {
        Task<ContractsSummaryDTO> GetContractsSummary();
        Task<PagedList<Contract>> GetAllContracts(Guid userId, ContractParameters parameters);
        Task<int> GetAllContractsForProcurementPlanCount(Guid procurementPlanId);
        Task<VendorProcurement> GetIndicatedVendor(Guid procurementPlanId, Guid vendorId);
        Task<int> PendingContractExists(Guid procurementPlanId);
        Task<ContractsSummaryDTO> GetContractSummaryForVendor(Guid vendorId);
        Task<VendorProfile> GetVendorProfile(Guid vendorId);
        Task<VendorBid> GetVendorBid(Guid procurementPlanId, Guid vendorId);
        Task<PagedList<Contract>> GetContractsForVendor(Guid vendorUserId, ResourceParameters parameters);
        Task<PagedList<Contract>> GetContractByProcurementPlanId(Guid procurementPlanId, ResourceParameters parameters);
        Task<IndividualContractDTO> GetContract(Guid Id);
        Task<IEnumerable<ProcurementPlanDocument>> GetDocuments(Guid procurementPlanId);

        Task<PagedList<Contract>> GetAwardedContracts(ResourceParameters parameters);
        Task<Contract> GetRejectedOrExpiredContract(Guid procurementPlanId);
        Task<PagedList<Contract>> GetAllContractsByVendor(Guid vendorId, ResourceParameters parameters);
        Task<bool> IsRecommendedVendorExists(Guid procurementPlanId);
        Task<Contract> GetPendingContract(Guid procurementPlanId);
        Task<PagedList<ProcurementContract>> GetProcurmentContract(ProcurementContractParameters parameters);
        Task<ProcurementContract> GetProcurmentContractById(Guid contractId);
    }
}
