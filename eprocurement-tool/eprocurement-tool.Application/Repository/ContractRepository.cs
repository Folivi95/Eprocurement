using AutoMapper;
using EGPS.Application.Helpers;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Repository
{
    public class ContractRepository : Repository<Contract>, IContractRepository
    {
        private readonly IMapper _mapper;

        public ContractRepository(EDMSDBContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<PagedList<Contract>> GetAllContracts(Guid userId, ContractParameters parameters)
        {
            var query = _context.Contracts as IQueryable<Contract>;

            if (!string.IsNullOrEmpty(parameters.Search) && !string.IsNullOrEmpty(parameters.SearchBy) && parameters.SearchBy.ToLower() == "description")
            {
                var search = parameters.Search.Trim();
                query = query.Where(x => x.Description.ToLower().Contains(parameters.Search.ToLower()));

            }
            if (!string.IsNullOrEmpty(parameters.Search) && !string.IsNullOrEmpty(parameters.SearchBy) && parameters.SearchBy.ToLower() == "referenceid")
            {
                var search = parameters.Search.Trim();
                query = query.Where(x => x.ReferenceId.ToLower().Contains(parameters.Search.ToLower()));
            }

            if (!string.IsNullOrEmpty(parameters.Search) && !string.IsNullOrEmpty(parameters.SearchBy) && parameters.SearchBy.ToLower() == "contractorname")
            {
                var search = parameters.Search.Trim();
                query = query.Where(x => x.Contractor.VendorProfile.CompanyName.ToLower().Contains(parameters.Search.ToLower()));
            }

            if (!string.IsNullOrEmpty(parameters.Status) && Enum.IsDefined(typeof(EContractStatus), parameters.Status.ToUpper()))
            {
                //get status id
                var type = parameters.Status.ParseStringToEnum(typeof(EContractStatus));
                query = query.Where(x => x.Status == (EContractStatus)type);
            }

            if (parameters.Date != null)
            {
                query = query.Where(x => x.StartDate.Value.Date == parameters.Date.Value.Date);
            }

            query = query.Include(c => c.Contractor).ThenInclude(v => v.VendorProfile).DefaultIfEmpty().AsNoTracking();

            var contracts = await PagedList<Contract>.Create(query, parameters.PageNumber, parameters.PageSize);

            return contracts;
        }

        public async Task<IndividualContractDTO> GetContract(Guid Id)
        {
            var contract = await _context.Contracts.Where(x => x.Id == Id)
                .Include(v => v.Contractor).ThenInclude(u => u.VendorProfile)
                .Include(p => p.ProcurementPlan).Select(c => new IndividualContractDTO()
                {
                    Id = c.Id,
                    Title = c.Title,
                    Contractor = _mapper.Map<VendorProfileForContractDTO>(c.Contractor.VendorProfile),
                    ContractNumber = c.ContractNumber,
                    EvaluationCurrency = c.EvaluationCurrency,
                    Description = c.Description,
                    Status = c.Status,
                    SignatureStatus = c.SignatureStatus,
                    EstimatedValue = c.EstimatedValue,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    CreatedById = c.UserId,
                    PercentageCompleted = c.PercentageCompletion,
                    ProcurementPlan = _mapper.Map<ProcurementPlanForContractDto>(c.ProcurementPlan)
                }).SingleOrDefaultAsync();

            return contract;
        }


        public async Task<ContractsSummaryDTO> GetContractsSummary()
        {
            var query = _context.Contracts.AsNoTracking();

            var totalContracts = await query.CountAsync();
            var totalAwarded = await query.Where(x => x.Status == EContractStatus.ACCEPTED).CountAsync();
            var totalSigned = await query.Where(x => x.SignatureStatus == ESignatureStatus.SIGNED).CountAsync();
            var totalUnsigned = await query.Where(x => x.SignatureStatus == ESignatureStatus.UNSIGNED).CountAsync();
            var totalPending = await query.Where(x => x.Status == EContractStatus.PENDING).CountAsync();
            var totalRejected = await query.Where(x => x.Status == EContractStatus.REJECTED).CountAsync();

            var summaryDetails = new ContractsSummaryDTO()
            {
                Total = totalContracts,
                Awarded = totalAwarded,
                Signed = totalSigned,
                Unsigned = totalUnsigned,
                Pending = totalPending,
                Rejected = totalRejected
            };

            return summaryDetails;
        }

        public async Task<int> GetAllContractsForProcurementPlanCount(Guid procurementPlanId)
        {
            var contracts = await _context.Contracts.Where(c => c.ProcurementPlanId == procurementPlanId).CountAsync();

            return contracts;
        }

        public async Task<VendorProcurement> GetIndicatedVendor(Guid procurementPlanId, Guid vendorId)
        {
            var procurementPlan = await _context.VendorProcurements.FirstOrDefaultAsync(x => x.ProcurementPlanId == procurementPlanId && x.VendorId == vendorId);

            return procurementPlan;
        }

        public async Task<int> PendingContractExists(Guid procurementPlanId)
        {
            var contracts = await _context.Contracts.Where(c => c.ProcurementPlanId == procurementPlanId && (c.Status == EContractStatus.ACCEPTED || c.Status == EContractStatus.PENDING)).CountAsync();

            return contracts;
        }

        public async Task<VendorProfile> GetVendorProfile(Guid vendorId)
        {
            var vendorProfile = await _context.VendorProfiles.SingleOrDefaultAsync(v => v.UserId == vendorId);

            return vendorProfile;
        }

        public async Task<VendorBid> GetVendorBid(Guid procurementPlanId, Guid vendorId)
        {
            var vendorBid = await _context.VendorBids.FirstOrDefaultAsync(x => x.ProcurementPlanId == procurementPlanId && x.VendorId == vendorId);

            return vendorBid;
        }

        public async Task<PagedList<Contract>> GetContractsForVendor(Guid vendorUserId, ResourceParameters parameters)
        {
            var query = _context.Contracts.Where(x => x.ContractorId == vendorUserId)
                            .Include(x => x.ProcurementPlan).ThenInclude(p => p.ProcurementCategory);

            var contracts = await PagedList<Contract>.Create(query, parameters.PageNumber, parameters.PageSize);

            return contracts;
        }

        public async Task<PagedList<Contract>> GetContractByProcurementPlanId(Guid procurementPlanId, ResourceParameters parameters)
        {
            var contractsQuery = _context.Contracts.Where(p => p.ProcurementPlanId == procurementPlanId)
                                        .Include(m => m.ProcurementPlan)
                                        .Include(x => x.Contractor).DefaultIfEmpty().AsNoTracking();

            var contracts = await PagedList<Contract>.Create(contractsQuery, parameters.PageNumber, parameters.PageSize);

            return contracts;
        }

        public async Task<IEnumerable<ProcurementPlanDocument>> GetDocuments(Guid procurementPlanId)
        {
            var activities = await _context.ProcurementPlanActivities.Where(p => p.ProcurementPlanId == procurementPlanId && p.Title == "Contract Signing").SingleOrDefaultAsync();



            if (activities == null)
            {
                return null;
            }

            var documents = await _context.ProcurementPlanDocuments.Where(a => a.ObjectId == activities.Id).ToListAsync();

            return documents;
        }
        public async Task<PagedList<Contract>> GetAwardedContracts(ResourceParameters parameters)
        {
            var type = EContractStatus.ACCEPTED;
            var awardedContracts = _context.Contracts.Where(x => x.Status == (EContractStatus)type);
            var pagedAwardedContracts = await PagedList<Contract>.Create(awardedContracts, parameters.PageNumber, parameters.PageSize);
            return pagedAwardedContracts;
        }

        public async Task<ContractsSummaryDTO> GetContractSummaryForVendor(Guid vendorId)
        {
            var query = _context.Contracts.Where(x => x.ContractorId == vendorId).AsNoTracking();

            var totalContracts = await query.CountAsync();
            var totalAwarded = await query.Where(x => x.Status == EContractStatus.ACCEPTED).CountAsync();
            var totalSigned = await query.Where(x => x.SignatureStatus == ESignatureStatus.SIGNED).CountAsync();
            var totalUnsigned = await query.Where(x => x.SignatureStatus == ESignatureStatus.UNSIGNED).CountAsync();
            var totalPending = await query.Where(x => x.Status == EContractStatus.PENDING).CountAsync();
            var totalRejected = await query.Where(x => x.Status == EContractStatus.REJECTED).CountAsync();

            var summaryDetails = new ContractsSummaryDTO()
            {
                Total = totalContracts,
                Awarded = totalAwarded,
                Signed = totalSigned,
                Unsigned = totalUnsigned,
                Pending = totalPending,
                Rejected = totalRejected
            };

            return summaryDetails;
        }

        public async Task<Contract> GetRejectedOrExpiredContract(Guid procurementPlanId)
        {
            var contract = await _context.Contracts.FirstOrDefaultAsync(c => c.ProcurementPlanId == procurementPlanId && (c.Status == EContractStatus.EXPIRED || c.Status == EContractStatus.REJECTED));

            return contract;
        }
        public async Task<PagedList<Contract>> GetAllContractsByVendor(Guid vendorId, ResourceParameters parameters)
        {
            var query = _context.Contracts.Where(x => x.ContractorId == vendorId);

            var contracts = await PagedList<Contract>.Create(query, parameters.PageNumber, parameters.PageSize);

            return contracts;
        }

        public async Task<bool> IsRecommendedVendorExists(Guid procurementPlanId)
        {
            return await _context.VendorBids.AnyAsync(x =>
                    x.ProcurementPlanId == procurementPlanId && x.Type == EVendorContractStatus.RECOMMENDED);
        }

        public async Task<Contract> GetPendingContract(Guid procurementPlanId)
        {
            var contracts = await _context.Contracts.SingleOrDefaultAsync(c => c.ProcurementPlanId == procurementPlanId && c.Status == EContractStatus.PENDING);

            return contracts;
        }

        public async Task<PagedList<ProcurementContract>> GetProcurmentContract(ProcurementContractParameters parameters)
        {
            var query = _context.Contracts.Where(x => x.Status == EContractStatus.ACCEPTED);

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim();
                query = query.Where(x => x.ProcurementPlan.Name.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(parameters.Category))
            {
                query = query.Where(x => x.ProcurementPlan.ProcurementCategory.Name == parameters.Category);
            }

            if (parameters.DateAwarded.HasValue)
            {
                query = query.Where(x => x.CreateAt.Date == parameters.DateAwarded.Value.Date);
            }

            var procurementPlan = query.OrderByDescending(x => x.CreateAt).Select(x => new ProcurementContract
            {
                Id = x.Id,
                Title = x.Title,
                DateAwarded = x.CreateAt,
                ExpiryDate = x.EndDate,
                Status = x.Status,
                EstimatedValue = x.EstimatedValue,
                EvaluationCurrency = x.EvaluationCurrency,
                Vendor = x.Contractor.VendorProfile.CompanyName,
                Procurement = new ContractProcurement
                {
                    Id = x.Id,
                    Name = x.ProcurementPlan.Name,
                    MinistryId = x.ProcurementPlan.MinistryId,
                    Description = x.ProcurementPlan.Description,
                    ProcurementCategoryId = x.ProcurementPlan.ProcurementCategoryId,
                    Category = new ProcurmentCategoryDTO
                    {
                        Name = x.ProcurementPlan.ProcurementCategory.Name
                    },
                    Ministry = new ProcurementMinistryDTO
                    {
                        Name = x.ProcurementPlan.Ministry.Name
                    }
                }
            });

            var procurementResponse =
                await PagedList<ProcurementContract>.Create(procurementPlan, parameters.PageNumber, parameters.PageSize);

            return procurementResponse;
        }

        public async Task<ProcurementContract> GetProcurmentContractById(Guid contractId)
        {
            var contract = await _context.Contracts.Where(x => x.Id == contractId).Select(x => new ProcurementContract
            {
                Id = x.Id,
                Title = x.Title,
                DateAwarded = x.CreateAt,
                ExpiryDate = x.EndDate,
                Status = x.Status,
                EstimatedValue = x.EstimatedValue,
                EvaluationCurrency = x.EvaluationCurrency,
                Vendor = x.Contractor.VendorProfile.CompanyName,
                Procurement = new ContractProcurement
                {
                    Id = x.Id,
                    Name = x.ProcurementPlan.Name,
                    MinistryId = x.ProcurementPlan.MinistryId,
                    Description = x.ProcurementPlan.Description,
                    ProcurementCategoryId = x.ProcurementPlan.ProcurementCategoryId,
                    Category = new ProcurmentCategoryDTO
                    {
                        Name = x.ProcurementPlan.ProcurementCategory.Name
                    },
                    Ministry = new ProcurementMinistryDTO
                    {
                        Name = x.ProcurementPlan.Ministry.Name
                    }
                }
            }).SingleOrDefaultAsync();

            return contract;
        }
    }
}
