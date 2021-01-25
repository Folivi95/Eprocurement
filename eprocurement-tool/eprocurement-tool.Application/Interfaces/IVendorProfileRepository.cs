using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IVendorProfileRepository : IRepository<VendorProfile>
    {
        //Task AddVendorDocument(VendorDocument vendorDocument);
        //Task<PagedList<VendorDirector>> GetVendorDirectors(VendorDirectorParameter parameter, Guid userId);
        Task<VendorProfile> GetVendorProfile(Guid userId);
        Task<PagedList<BusinessService>> GetBusinessServicesForVendor(BusinessServicesParameter parameters, Guid userId);
        Task<PagedList<VendorAttestation>> GetAttestationsForVendor(VendorAttestationParameter parameter, Guid userId);
        Task<PagedList<VendorDocument>> GetDocumentsForVendor(VendorDocumentParameter parameter, Guid userId);
        Task<VendorDirector> GetVendorDirector(Guid userId);
        Task<PagedList<VendorDirector>> GetVendorDirectorWithCertificates(VendorDirectorParameter parameter, Guid userId);
        Task AddVendorDirector(VendorDirector vendorDirector);
        Task<VendorSummaryDto> GetVendorSummaryDetails();
        Task<PagedList<User>> GetAllVendors(Guid userId, GetAllVendorParameters parameters);

        Task<PagedList<VendorProfile>> SearchVendor(VendorParameters parameter);
        Task<InitiatePaymentDTO> InitiatePayment(Guid vendorProfileId, Guid userId, string publicKey, string callbackUrl);
        Task<UpdatePaymentDTO> UpdatePayment(VendorProfile vendorProfile, User user, string transactionId);
        Task<VerifyPaymentDTO> VerifyPayment(VendorProfile vendorProfile, string publicKey, string requeryUrl, string transactionId);
    }
}
