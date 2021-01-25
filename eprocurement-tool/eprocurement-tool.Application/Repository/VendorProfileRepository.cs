using EGPS.Application.Helpers;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EGPS.Application.Repository
{
    public class VendorProfileRepository : Repository<VendorProfile>, IVendorProfileRepository
    {
        public VendorProfileRepository(EDMSDBContext context)
            : base(context)
        {
            
        }

        public Task<PagedList<BusinessService>> GetBusinessServicesForVendor(BusinessServicesParameter parameters, Guid userId)
        {
            //get business services using the userId supplied in the API request
            var businessServicesQuery = _context.VendorServices.Where(s => s.UserID == userId).Select(x => x.BusinessServices).AsNoTracking();

            //return business services.
            var businessServices = PagedList<BusinessService>.Create(businessServicesQuery, parameters.PageNumber, parameters.PageSize);

            return businessServices;
        }

        public Task<PagedList<VendorAttestation>> GetAttestationsForVendor(VendorAttestationParameter parameter, Guid userId)
        {
            var attestationsQuery = _context.VendorAttestations.Where(x => x.UserId == userId).OrderByDescending(x => x.CreateAt);

            var attestations = PagedList<VendorAttestation>.Create(attestationsQuery, parameter.PageNumber, parameter.PageSize);

            return attestations;
        }


        public async Task<VendorProfile> GetVendorProfile(Guid userId)
        {
            return await _context.VendorProfiles.Where(vp => vp.UserId == userId).Include(x => x.RegistrationPlan).DefaultIfEmpty().FirstOrDefaultAsync();
        }

        public async Task<VendorDirector> GetVendorDirector(Guid Id)
        {
            return await _context.VendorDirectors.FirstOrDefaultAsync(vd => vd.Id == Id && !vd.Deleted);
        }

        public Task<PagedList<VendorDocument>> GetDocumentsForVendor(VendorDocumentParameter parameter, Guid userId)
        {
            var vendorDocumentQuery = _context.VendorDocuments.Where(x => x.UserId == userId && !x.Deleted);

            var vendorDocuments = PagedList<VendorDocument>.Create(vendorDocumentQuery, parameter.PageNumber, parameter.PageSize);

            return vendorDocuments;
        }

        public Task<PagedList<VendorDirector>> GetVendorDirectorWithCertificates(VendorDirectorParameter parameter, Guid userId)
        {
            var vendorDirectorQuery = _context.VendorDirectors.Where(x => x.UserId == userId && !x.Deleted).Include(x => x.Certifications);

            var vendorDirectors = PagedList<VendorDirector>.Create(vendorDirectorQuery, parameter.PageNumber, parameter.PageSize);

            return vendorDirectors;
        }

        public async Task AddVendorDirector(VendorDirector vendorDirector)
        {
            if (vendorDirector == null)
            {
                throw new ArgumentNullException(nameof(vendorDirector));
            }

            await _context.VendorDirectors.AddAsync(vendorDirector);
        }

        public async Task<VendorSummaryDto> GetVendorSummaryDetails()
        {
            var query = _context.VendorProfiles.AsNoTracking();

            var totalVendors = await query.CountAsync();
            var totalActiveVendors = await query.Where(x => x.Status == EVendorStatus.APPROVED).CountAsync();
            var totalPendingVendors = await query.Where(x => x.Status == EVendorStatus.PENDING).CountAsync();
            var rejectedVendors = await query.Where(v => v.Status == EVendorStatus.REJECTED).CountAsync();
            var blacklistedVendors = await query.Where(v => v.Status == EVendorStatus.BLACKLISTED).CountAsync();

            var summaryDetails = new VendorSummaryDto()
            {
                Total = totalVendors,
                ActiveTotal = totalActiveVendors,
                PendingTotal = totalPendingVendors,
                RejectedTotal = rejectedVendors,
                BlacklistedTotal = blacklistedVendors,
            };

            return summaryDetails;
        }

        public async Task<PagedList<User>> GetAllVendors(Guid userId, GetAllVendorParameters parameters)
        {
            

            var query = _context.Users as IQueryable<User>;

            if (parameters.Status == EVendorStatus.PENDING || parameters.Status == EVendorStatus.APPROVED || parameters.Status == EVendorStatus.REJECTED || parameters.Status == EVendorStatus.BLACKLISTED)
            {
                if (!String.IsNullOrEmpty(parameters.RegisterPlanTitle))
                {
                    query = (from v in _context.Users
                             where v.Status == EStatus.ENABLED && v.UserType == EUserType.VENDOR
                             join p in _context.VendorProfiles on v.Id equals p.UserId
                             where p.Status == parameters.Status && p.RegistrationPlan.Title.ToLower().Contains(parameters.RegisterPlanTitle.ToLower())
                             select v).Include(x => x.VendorProfile).ThenInclude(x => x.RegistrationPlan).AsNoTracking();
                }
                else
                {
                    (from v in _context.Users
                     where v.Status == EStatus.ENABLED && v.UserType == EUserType.VENDOR
                     join p in _context.VendorProfiles on v.Id equals p.UserId
                     where p.Status == parameters.Status
                     select v).Include(x => x.VendorProfile).ThenInclude(x => x.RegistrationPlan).AsNoTracking();
                }
                
            }


            else
            {
                if (!String.IsNullOrEmpty(parameters.RegisterPlanTitle))
                {
                    query = (from v in _context.Users
                             where v.Status == EStatus.ENABLED && v.UserType == EUserType.VENDOR
                             join p in _context.VendorProfiles on v.Id equals p.UserId
                             where p.RegistrationPlan.Title.ToLower().Contains(parameters.RegisterPlanTitle.ToLower())
                             select v).Include(x => x.VendorProfile).ThenInclude(x => x.RegistrationPlan).AsNoTracking();
                }
                else
                {
                    query = (from v in _context.Users
                             where v.UserType == EUserType.VENDOR
                             join p in _context.VendorProfiles on v.Id equals p.UserId
                             select v).Include(x => x.VendorProfile).ThenInclude(x => x.RegistrationPlan).AsNoTracking();
                }    
            }

            if (!String.IsNullOrEmpty(parameters.CompanyName))
            {
                query = (from v in _context.Users
                         where v.Status == EStatus.ENABLED && v.UserType == EUserType.VENDOR
                         join p in _context.VendorProfiles on v.Id equals p.UserId
                         where p.CompanyName.ToLower().Contains(parameters.CompanyName.ToLower())
                         select v).Include(x => x.VendorProfile).ThenInclude(x => x.RegistrationPlan).AsNoTracking();
            }


            //var vendorUsersQuery = query.Where(x => x.UserType == EUserType.VENDOR).Include(x => x.VendorProfile).ThenInclude(x => x.RegistrationPlan).DefaultIfEmpty();

            var vendorUsers = PagedList<User>.Create(query, parameters.PageNumber, parameters.PageSize);

            return (await vendorUsers);
        } 

        public async Task<PagedList<VendorProfile>> SearchVendor( VendorParameters parameter)
        {
           
            var query =  _context.VendorProfiles.OrderByDescending(a=>a.CreateAt).AsQueryable();
            if (!string.IsNullOrEmpty(parameter.Name))
            {
                query = query.Where(a => a.User.FirstName.ToLower()
                                             .Contains(parameter.Name.ToLower())
                                         || a.User.LastName.ToLower()
                                             .Contains(parameter.Name.ToLower()
                                             )
                                         || a.CompanyName.ToLower()
                                             .Contains(parameter.Name.ToLower()
                                             )
                );
            }

            if (!string.IsNullOrEmpty(parameter.RegisterId))
            {
                query = query.Where(a => a.CACRegistrationNumber.ToLower()
                        .Contains(parameter.RegisterId.ToLower()))
                    ;
            }
            var ministries = await PagedList<VendorProfile>.Create(query, parameter.PageNumber, parameter.PageSize);
            return ministries;
        }

        public async Task<InitiatePaymentDTO> InitiatePayment(Guid vendorProfileId, Guid userId, string publickey, string callbackUrl)
        {
            var vendorProfileWithRegPlan = await _context.VendorProfiles.Where(v => v.Id == vendorProfileId)
                                .Include(c => c.RegistrationPlan).FirstOrDefaultAsync();

            var userDetails = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

            if (userDetails == null || vendorProfileWithRegPlan == null)
            {
                return null;
            }

            var paymentDetails = new InitiatePaymentDTO();

            decimal amount = vendorProfileWithRegPlan.RegistrationPlan.Fee;
            string transactionId = TransactionHelper.GenerateRandomNumber(9);

            //start working on generating hash
            StringBuilder sb = new StringBuilder();
            sb.Append(publickey);
            sb.Append(amount);
            sb.Append(callbackUrl);
            sb.Append(paymentDetails.Country);
            sb.Append(paymentDetails.Currency);
            sb.Append(userDetails.Email ?? " ");
            sb.Append(userDetails.FirstName ?? " ");
            sb.Append(userDetails.LastName ?? " ");
            sb.Append(userDetails.Phone ?? " ");
            sb.Append(transactionId);

            //compute hash with the details 
            string hash = TransactionHelper.ComputeSHA256Hash(sb.ToString());

            try
            {
                //update transactionId in vendor profile table
                vendorProfileWithRegPlan.RegistrationPaymentId = transactionId;

                _context.VendorProfiles.Update(vendorProfileWithRegPlan);
                await _context.SaveChangesAsync();

                //update payment details dto to send back to the controller
                paymentDetails.Hash = hash;
                paymentDetails.VendorProfileId = vendorProfileId;
                paymentDetails.PublicKey = publickey;
                paymentDetails.TransactionId = transactionId;
                paymentDetails.CallbackUrl = callbackUrl;
                paymentDetails.FirstName = userDetails.FirstName ?? " ";
                paymentDetails.LastName = userDetails.LastName ?? " ";
                paymentDetails.Email = userDetails.Email ?? " ";
                paymentDetails.PhoneNumber = userDetails.Phone ?? " ";
                paymentDetails.Amount = amount.ToString("G");

                return paymentDetails;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to update vendor profiles table with transactionId: {transactionId} " +
                    $"due to {ex.Message}");

                return null;
            }
        }

        public async Task<UpdatePaymentDTO> UpdatePayment(VendorProfile vendorProfile, User user, string transactionId)
        {
            UpdatePaymentDTO detailsToReturn = new UpdatePaymentDTO();
            // update payment details in database for the vendor
            try
            {
                var registrationPlan = await _context.RegistrationPlans.Where(m => m.Id == vendorProfile.RegistrationPlanId).FirstOrDefaultAsync();

                if (registrationPlan == null)
                {
                    return null;
                }


                vendorProfile.RegistrationPaymentStatus = EPaymentStatus.PAID;
                vendorProfile.Status = EVendorStatus.PENDING;
                vendorProfile.IsRegistrationComplete = true;
                _context.VendorProfiles.Update(vendorProfile);

                await _context.SaveChangesAsync();

                
                detailsToReturn = new UpdatePaymentDTO
                {
                    FirstName = user.FirstName ?? " ",
                    LastName = user.LastName ?? " ",
                    Email = user.Email ?? " ",
                    PhoneNumber = user.Phone ?? " ",
                    Amount = registrationPlan.Fee,
                    TransactionId = vendorProfile.RegistrationPaymentId,
                    PaymentStatus = EPaymentStatus.PAID
                };

                return detailsToReturn;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Vendor registration payment status update failed due to {ex.Message}");
                return null;
            }

        }

        public async Task<VerifyPaymentDTO> VerifyPayment(VendorProfile vendorProfile, string publicKey, string requeryUrl, string transactionId)
        {
            string content = $"{{\"transactionId\":\"{transactionId}\"," +
                        $"\"publicKey\":\"{publicKey}\"}}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.PostAsync(requeryUrl, new StringContent(content, Encoding.UTF8, "application/json"));

                var paymentRes = new VerifyPaymentDTO();

                if (res.IsSuccessStatusCode)
                {
                    using (Stream resStream = await res.Content.ReadAsStreamAsync())
                    {
                        paymentRes = await JsonSerializer.DeserializeAsync<VerifyPaymentDTO>(resStream);
                    }

                    if (paymentRes.data.payment.paymentResponseCode.Equals("000"))
                    {
                        return paymentRes;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return null;
            
        }
    }
}
