using EGPS.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using System.Threading.Tasks;
using System.Linq;
using EGPS.Domain.Enums;
using EGPS.Application.Helpers;
using Microsoft.EntityFrameworkCore;
using EGPS.Application.Models;
using AutoMapper;
using System.Security.Cryptography.X509Certificates;

namespace EGPS.Application.Repository
{
    public class ProcurementPlanRepository : Repository<ProcurementPlan>, IProcurementPlanRepository
    {
        private readonly IMapper _mapper;

        public ProcurementPlanRepository(EDMSDBContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProcurementPlanType>> GetProcurementPlanTypes(string type)
        {
            var procurementPlanTypes = _context.ProcurementPlanTypes as IQueryable<ProcurementPlanType>;

            if (!string.IsNullOrWhiteSpace(type) && Enum.IsDefined(typeof(EPprocurementPlanTask), type.ToUpper()))
            {
                var enumType = type.ParseStringToEnum(typeof(EPprocurementPlanTask));
                procurementPlanTypes = procurementPlanTypes.Where(n => n.ProcurementPlanTask == (EPprocurementPlanTask)enumType);
            }


            return await procurementPlanTypes.ToListAsync();
        }

        public async Task AddProcurementPlanDocument(IEnumerable<ProcurementPlanDocument> documents)
        {
            await _context.ProcurementPlanDocuments.AddRangeAsync(documents);
        }

        public async Task<bool> IsProcurementPlanActivityExist(Guid procurementPlanActivityId)
        {
            return await _context.ProcurementPlanActivities.AnyAsync(x => x.Id == procurementPlanActivityId);
        }

        public async Task<ProcurementPlanActivity> GetProcurementPlanActivity(Guid procurementPlanActivityId)
        {
            return await _context.ProcurementPlanActivities.FirstOrDefaultAsync(x => x.Id == procurementPlanActivityId);
        }

        public async Task<bool> IsProcurementPlanActivityTitleExists(string procurementPlanActivityTitle)
        {
            return await _context.ProcurementPlanActivities.AnyAsync(x => x.Title.ToLower() == procurementPlanActivityTitle.ToLower());
        }

        public async Task<bool> IsProcurementPlanDocumentExist(Guid procurementPlanDocumentId)
        {
            return await _context.ProcurementPlanDocuments.AnyAsync(x => x.Id == procurementPlanDocumentId);
        }

        public async Task AddProcurementPlanReview(Review review)
        {
            await _context.Reviews.AddAsync(review);
        }

        public async Task AddProcurementPlanReviewDocument(ProcurementPlanDocument document)
        {
            await _context.ProcurementPlanDocuments.AddAsync(document);
        }

        public async Task AddNoticeInformation(NoticeInformation noticeInformation)
        {
            await _context.NoticeInformations.AddAsync(noticeInformation);
        }

        public void UpdateNoticeInformation(NoticeInformation noticeInformation)
        {
            _context.NoticeInformations.Update(noticeInformation);
        }

        public async Task<NoticeInformation> GetNoticeInformation(Guid noticeInformationId)
        {
            var noticeInformation = _context.NoticeInformations.Where(x => x.Id == noticeInformationId).Include(x => x.ProcurementPlan);

            return await noticeInformation.SingleOrDefaultAsync();
        }

        public async Task<ProcurementPlan> GetProcurementPlan(Guid procurementPlanId)
        {
            var procurementPlan = _context.ProcurementPlans
                .Where(x => x.Id == procurementPlanId)
                .Include(x => x.ProcurementPlanActivities)
                .Include(x => x.ProcurementMethod)
                .Include(x => x.ProcurementProcess)
                .Include(x => x.QualificationMethod)
                .Include(x => x.ReviewMethod)
                .Include(x => x.ProcurementCategory)
                .Include(m => m.Ministry);

            return await procurementPlan.SingleOrDefaultAsync();
        }

        public void UpdateProcurementPlanActivity(ProcurementPlanActivity procurementPlanActivity)
        {
            _context.Update(procurementPlanActivity);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<ProcurementPlanDocument>> GetProcurementPlanDocumentWithObjectType(Guid procurementPlanActivityId, EDocumentObjectType? objectType)
        {
            if (objectType.HasValue)
            {
                var procurementPlanDocumentWithObjectType = await _context.ProcurementPlanDocuments.Where(x => x.ObjectId == procurementPlanActivityId && x.ObjectType == objectType).ToListAsync();
                return procurementPlanDocumentWithObjectType;
            }

            var procurementPlanDocument = await _context.ProcurementPlanDocuments.Where(x => x.ObjectId == procurementPlanActivityId).ToListAsync();

            return procurementPlanDocument;
        }

        public async Task<IEnumerable<ProcurementPlanDocument>> GetProcurementPlanDocument(Guid procurementPlanActivityId)
        {
            var procurementPlanDocument = await _context.ProcurementPlanDocuments.Where(x => x.ObjectId == procurementPlanActivityId).ToListAsync();

            return procurementPlanDocument;
        }

        public async Task<IEnumerable<Review>> GetProcurementPlanActivityReview(Guid objectId)
        {
            var review = await _context.Reviews.Where(x => x.ObjectId == objectId).Include(x => x.CreatedBy).ToListAsync();
            return review;
        }
        public async Task AddDatasheet(Datasheet datasheet)
        {
            await _context.Datasheets.AddAsync(datasheet);
        }

        public void UpdateDatasheet(Datasheet datasheet)
        {
            _context.Datasheets.Update(datasheet);
        }

        public async Task<Datasheet> GetProcurementDatasheet(Guid procurementPlanActivityId)
        {
            var datasheet = await _context.Datasheets.FirstOrDefaultAsync(x => x.ProcurementPlanActivityId == procurementPlanActivityId);

            return datasheet;
        }
        public async Task<bool> IsIndicatedVendor(Guid vendorId, Guid procurementPlanId)
        {
            return await _context.VendorBids.AnyAsync(x => x.VendorId == vendorId && x.ProcurementPlanId == procurementPlanId);
        }

        public async Task AddIndicatedVendor(VendorProcurement vendorProcurement)
        {
            await _context.VendorProcurements.AddAsync(vendorProcurement);
        }

        public async Task RemoveIndicatedVendor(ICollection<Guid?> vendorId)
        {
            var vendorProcurements = await _context.VendorProcurements.Where(v => vendorId.Contains(v.VendorId)).ToListAsync();

            _context.VendorProcurements.RemoveRange(vendorProcurements);
        }

        public async Task<List<Guid>> GetIndicatedVendors(Guid procurementPlanId)
        {
            var procurementPlan = await _context.VendorBids.Where(x => x.ProcurementPlanId == procurementPlanId && x.Type == EVendorContractStatus.INTERESTED).Select(v => v.VendorId).ToListAsync();

            return procurementPlan;
        }

        public async Task<ProcurementMethod> GetProcurementMethod(Guid procurementMethodId)
        {
            var procurementMethod = await _context.ProcurementMethods.Where(p => p.Id == procurementMethodId).FirstOrDefaultAsync();

            return procurementMethod;
        }

        public async Task<QualificationMethod> GetQualificationMethod(Guid qualificationMethodId)
        {
            var qualificationMethod = await _context.QualificationMethods.Where(p => p.Id == qualificationMethodId).FirstOrDefaultAsync();

            return qualificationMethod;
        }

        public async Task<ProcurementCategory> GetProcurementCategory(Guid procurementCategoryId)
        {
            var procurementCategory = await _context.ProcurementCategories.Where(p => p.Id == procurementCategoryId).FirstOrDefaultAsync();

            return procurementCategory;
        }

        public async Task<ReviewMethod> GetReviewMethod(Guid reviewMethodId)
        {
            var reviewMethod = await _context.ReviewMethods.Where(p => p.Id == reviewMethodId).FirstOrDefaultAsync();

            return reviewMethod;
        }

        public async Task<ProcurementProcess> GetProcurementProcess(Guid procurementProcessId)
        {
            var procurementProcess = await _context.ProcurementProcesses.Where(p => p.Id == procurementProcessId).FirstOrDefaultAsync();

            return procurementProcess;
        }

        public async Task<Review> GetProcurementReviewById(Guid reviewId)
        {
            var review = await _context.Reviews.Where(x => x.Id == reviewId).Include(x => x.CreatedBy)
                .SingleOrDefaultAsync();

            return review;
        }

        public void UpdateVendorProcurements(VendorProcurement vendorProcurement)
        {
            _context.Update(vendorProcurement);
        }

        public async Task<PagedList<ContractsDTO>> GetContracts(Guid procurementPlanId, ContractsParameters parameters)
        {


            var query = _context.Contracts as IQueryable<Contract>;

            //query by status if parameter value is present
            if (!String.IsNullOrEmpty(parameters.Status.ToString()))
            {
                query = query.Where(x => x.Status.Equals(parameters.Status));
            }

            //query by procurement signatureStatus if parameter value is present
            if (!String.IsNullOrEmpty(parameters.SignatureStatus.ToString()))
            {
                query = query.Where(x => x.SignatureStatus.Equals(parameters.SignatureStatus));
            }

            var contracts = query.Where(x => x.ProcurementPlanId == procurementPlanId)
                                                .Include(v => v.Contractor).ThenInclude(u => u.VendorProfile)
                                                .Include(p => p.ProcurementPlan).Select(c => new ContractsDTO()
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
                                                })
                                                .AsNoTracking();

            var contractList = await PagedList<ContractsDTO>.Create(contracts, parameters.PageNumber, parameters.PageSize);
            return contractList;
        }

        public async Task<bool> IsProcurementPlanExists(Guid procurementPlanId)
        {
            return await _context.ProcurementPlans.AnyAsync(x => x.Id == procurementPlanId);
        }

        public async Task AddProcurementPlanActivity(List<ProcurementPlanActivity> activities)
        {
            await _context.ProcurementPlanActivities.AddRangeAsync(activities);
        }

        public async Task<IEnumerable<ProcurementCategory>> GetAllProcurementCategories()
        {
            return await _context.ProcurementCategories.ToListAsync();
        }

        public async Task<IEnumerable<ProcurementMethod>> GetAllProcurementMethods()
        {
            return await _context.ProcurementMethods.ToListAsync();
        }

        public async Task<IEnumerable<ProcurementProcess>> GetAllProcurementProcesses()
        {
            return await _context.ProcurementProcesses.ToListAsync();
        }

        public async Task<IEnumerable<QualificationMethod>> GetAllQualificationMethods()
        {
            return await _context.QualificationMethods.ToListAsync();
        }

        public async Task<IEnumerable<ReviewMethod>> GetAllReviewMethods()
        {
            return await _context.ReviewMethods.ToListAsync();
        }

        public async Task<ProcurementPlanNumber> GetProcurementPlanNumber(ProcurementPlanNumberParameters parameters)
        {
            if (!String.IsNullOrEmpty(parameters.Ministry) && !String.IsNullOrEmpty(parameters.ProcurementCategory)
                && !String.IsNullOrEmpty(parameters.ProcurementMethod))
            {
                var stateCode = "DS";
                var joiner = "/";
                var ministryCode = parameters.Ministry;
                var procurementCategoryCode = string.Concat(parameters.ProcurementCategory.Where(c => c >= 'A' && c <= 'Z'));

                //remove bracket from procurement method
                int i = parameters.ProcurementMethod.IndexOf('(');
                var procurmentMethodCode = "";

                if (i >= 0)
                {
                    procurmentMethodCode = parameters.ProcurementMethod.Remove(i);
                    procurmentMethodCode = string.Concat(procurmentMethodCode.Where(c => c >= 'A' && c <= 'Z'));
                }
                else
                {
                    procurmentMethodCode = string.Concat(parameters.ProcurementMethod.Where(c => c >= 'A' && c <= 'Z'));
                }

                var serialNumber = await _context.ProcurementPlanNumbers.Where(x => x.MinistryCode == ministryCode
                                          && x.ProcurementCategoryCode == procurementCategoryCode
                                          && x.ProcurementMethodCode == procurmentMethodCode).CountAsync();

                var year = DateTime.Now.Year;

                StringBuilder planNumber = new StringBuilder();
                planNumber.Append(stateCode);
                planNumber.Append(joiner);
                planNumber.Append(ministryCode);
                planNumber.Append(joiner);
                planNumber.Append(procurementCategoryCode);
                planNumber.Append(joiner);
                planNumber.Append(procurmentMethodCode);
                planNumber.Append(joiner);
                planNumber.Append(++serialNumber);
                planNumber.Append(joiner);
                planNumber.Append(year);

                //save changes to db
                var newPlanNumber = new ProcurementPlanNumber()
                {
                    StateCode = stateCode,
                    MinistryCode = ministryCode,
                    ProcurementCategoryCode = procurementCategoryCode,
                    ProcurementMethodCode = procurmentMethodCode,
                    SerialNumber = serialNumber,
                    Year = year,
                    PlanNumber = planNumber.ToString()
                };

                try
                {
                    await _context.ProcurementPlanNumbers.AddAsync(newPlanNumber);
                    await _context.SaveChangesAsync();
                    return newPlanNumber;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<NoticeInformation> GetNoticeInformationForProcurementPlan(Guid procurementPlanId)
        {
            var noticeInformation = await _context.NoticeInformations.Where(x => x.ProcurementPlanId == procurementPlanId).SingleOrDefaultAsync();

            return noticeInformation;
        }

        public async Task RemoveProcurementPlanDocument(IEnumerable<Guid?> documentId)
        {

            foreach (var id in documentId)
            {
                var document = await _context.ProcurementPlanDocuments.FindAsync(id);
                if (document != null)
                {
                    _context.ProcurementPlanDocuments.Remove(document);
                }
            }

        }


        public async Task<bool> IsProcurementPlanActivityDocumentExists(Guid procurementPlanActivityId)
        {
            return await _context.ProcurementPlanDocuments.AnyAsync(x => x.ObjectId == procurementPlanActivityId);
        }

        public async void UpdateProcurementPlanActivityRevisedDate(Guid procurementPlanActivityId)
        {

            var procurementPlanActivity = await GetProcurementPlanActivity(procurementPlanActivityId);
            if (procurementPlanActivity != null)
            {
                procurementPlanActivity.RevisedDate = DateTime.Now;
                _context.ProcurementPlanActivities.Update(procurementPlanActivity);
            }
        }

        public void UpdateVendorBids(VendorBid vendorBid)
        {
            _context.VendorBids.Update(vendorBid);
        }

        public async Task<List<Guid>> GetBidVendors(Guid procurementPlanId)
        {
            var vendorBids = await _context.VendorBids.Where(x => x.ProcurementPlanId == procurementPlanId && x.Type == EVendorContractStatus.PROCESSING).Select(v => v.VendorId).ToListAsync();

            return vendorBids;
        }

        public async Task<PagedList<ProcurementPlanActivity>> GetProcurementPlanActivityByTitle(string title, Guid procurementPlanId, ResourceParameters parameters)
        {
            var planActivitiesQuery = _context.ProcurementPlanActivities.Where(x => x.Title.Trim().ToLower().Contains(title.ToLower()) && x.ProcurementPlanId == procurementPlanId)
                                        .Include(x => x.ProcurementPlan)
                                        .ThenInclude(p => p.ProcurementProcess)
                                        .Include(x => x.ProcurementPlanDocuments)
                                        .Include(x => x.Reviews).ThenInclude(x => x.CreatedBy)
                                        .AsNoTracking();

            var planActivities = await PagedList<ProcurementPlanActivity>.Create(planActivitiesQuery, parameters.PageNumber, parameters.PageSize);

            return planActivities;
        }


        public async Task<PagedList<ProcurementPlanActivity>> GetProcurementPlanActivityByBidTitle(string title, BidsParameters parameters)
        {
            var planActivitiesQuery = _context.ProcurementPlanActivities.Where(x => x.Title.Trim().ToLower().Contains(title.ToLower()))
                                        .Include(x => x.ProcurementPlan)
                                        .ThenInclude(p => p.ProcurementProcess)
                                        .Include(x => x.ProcurementPlanDocuments)
                                        .Include(x => x.Reviews).ThenInclude(x => x.CreatedBy)
                                        .DefaultIfEmpty().AsNoTracking();


            if (!String.IsNullOrEmpty(parameters.Title))
            {
                planActivitiesQuery = planActivitiesQuery.Where(x => x.ProcurementPlan.Name.Contains(parameters.Title));
            }


            if (!String.IsNullOrEmpty(parameters.Process))
            {
                planActivitiesQuery = planActivitiesQuery.Where(x => x.ProcurementPlan.ProcurementProcess.Name.Contains(parameters.Process));
            }


            if (!string.IsNullOrEmpty(parameters.ExpiryDate.ToString()))
            {
                planActivitiesQuery = planActivitiesQuery.Where(x => x.EndDate.ToString() == parameters.ExpiryDate.ToString());
            }


            var planActivities = await PagedList<ProcurementPlanActivity>.Create(planActivitiesQuery, parameters.PageNumber, parameters.PageSize);

            return planActivities;
        }


        public async Task<ProcurementPlanActivity> GetVendorPlanActivityByBidTitle(string title, Guid procurementPlanId)
        {
            var planActivitiesQuery = await _context.ProcurementPlanActivities.Where(x => x.Title.Trim().ToLower().Contains(title.ToLower()) && x.ProcurementPlanId == procurementPlanId).Include(x => x.ProcurementPlan).SingleOrDefaultAsync();
            return planActivitiesQuery;
        }

        public async Task<DateTime> GetBidExpiryDate(Guid procurementPlanActivityId)
        {
            var datasheetQuery = await _context.Datasheets.Where(x => x.ProcurementPlanActivityId == procurementPlanActivityId).Select(x => x.SubmissionDeadline).SingleOrDefaultAsync();
            return datasheetQuery.Value;
        }


        public async Task<PagedList<VendorBid>> GetBiddersForProcurementPlan(Guid userId, ResourceParameters parameters)
        {
            var query = _context.VendorBids.Where(x => x.VendorId == userId && x.Type == EVendorContractStatus.PROCESSING)
                            .Include(x => x.Vendor).ThenInclude(x => x.VendorProfile).DefaultIfEmpty().AsNoTracking();

            var vendorBids = await PagedList<VendorBid>.Create(query, parameters.PageNumber, parameters.PageSize);

            return vendorBids;
        }

        public async Task<PagedList<NoticeInformation>> GetNoticeInformation(SpecialNoticeInformationParameters parameters)
        {
            var query = _context.NoticeInformations as IQueryable<NoticeInformation>;

            //query by title if parameter value is present
            if (!String.IsNullOrEmpty(parameters.Title))
            {
                query = query.Where(x => x.Title.Contains(parameters.Title));
            }

            //query by procurement category if parameter value is present
            if (!String.IsNullOrEmpty(parameters.ProcurementCategory))
            {
                query = query.Where(x => x.ProcurementPlan.ProcurementCategory.Name.Contains(parameters.ProcurementCategory));
            }

            //query by ministry if parameter value is present
            if (!String.IsNullOrEmpty(parameters.Ministry))
            {
                query = query.Where(x => x.ProcurementPlan.Ministry.Name.Contains(parameters.Ministry));
            }

            //query by expiry date if parameter value is present. It checks if expiry date has a value
            if (parameters.ExpiryDate.GetHashCode() != 0 && parameters.ExpiryDate != null)
            {
                query = query.Where(x => x.SubmissionDeadline.Date <= parameters.ExpiryDate.Value.Date);
            }

            var noticeInformationQuery = query.Include(x => x.ProcurementPlan)
                                                .ThenInclude(m => m.Ministry)
                                                .Include(c => c.ProcurementPlan).ThenInclude(p => p.ProcurementCategory)
                                                .AsNoTracking();

            var noticeInformation = await PagedList<NoticeInformation>.Create(noticeInformationQuery, parameters.PageNumber, parameters.PageSize);

            return noticeInformation;
        }

        public async Task<IEnumerable<ProcurementPlanActivity>> GetProcurementPlanActivities(Guid procurementPlanId)
        {
            return await _context.ProcurementPlanActivities.Where(a => a.ProcurementPlanId == procurementPlanId).ToListAsync();
        }

        public async Task<ProcurementPlanDocument> GetProcurementPlanActivityDocument(Guid documentId)
        {
            var procurementPlanDocument = await _context.ProcurementPlanDocuments.SingleOrDefaultAsync(d => d.Id == documentId);
            return procurementPlanDocument;
        }
        public async Task<IEnumerable<VendorBid>> GetProccessingBidVendors(ICollection<Guid?> vendorId)
        {
            var vendorBids = await _context.VendorBids.Where(v => vendorId.Contains(v.VendorId) && v.Type == EVendorContractStatus.PROCESSING).ToListAsync();

            return vendorBids;
        }

        public async Task<bool> IsBidVendor(Guid vendorId, Guid procurementPlanId)
        {
            return await _context.VendorBids.AnyAsync(x => x.VendorId == vendorId && x.ProcurementPlanId == procurementPlanId);
        }

        public async Task AddBidVendor(VendorBid vendorBid)
        {
            await _context.VendorBids.AddAsync(vendorBid);
        }

        public async Task<Contract> GetContract(Guid procurementPlanId)
        {
            var contract = await _context.Contracts.Where(x => x.ProcurementPlanId == procurementPlanId && x.Status == EContractStatus.PENDING).SingleOrDefaultAsync();

            return contract;
        }

        public async Task<PagedList<VendorBid>> GetBidsForVendor(Guid? userId, BidsTableParameters parameters)
        {
            var bids = _context.VendorBids as IQueryable<VendorBid>;

            bids = bids.OrderByDescending(x => x.ExpiryDate);

            if (userId != null)
                bids = bids.Where(x => x.VendorId == userId && x.Type != EVendorContractStatus.INTERESTED);

            bids = bids.Include(x => x.ProcurementPlan);


            if (parameters.ExpiryDate.HasValue)
                bids = bids.Where(x => x.ExpiryDate.Value.Date == parameters.ExpiryDate);

            if (!string.IsNullOrEmpty(parameters.Process))
                bids = bids.Where(x => x.ProcurementType.Trim().ToLower() == parameters.Process);

            if (!string.IsNullOrEmpty(parameters.Search))
                bids = bids.Where(x => x.ProcurementPlan.Name.ToLower().Contains(parameters.Search.ToLower()));


            if (parameters.Type.HasValue)
                bids = bids.Where(x => x.Type == parameters.Type);

            var vendorBids = await PagedList<VendorBid>.Create(bids, parameters.PageNumber, parameters.PageSize);


            return vendorBids;

        }

        public async Task<IEnumerable<VendorBid>> GetInterestedVendorBids(Guid procurementPlanId)
        {
            var vendorBids = await _context.VendorBids.Where(x => x.ProcurementPlanId == procurementPlanId && x.Type == EVendorContractStatus.INTERESTED).ToListAsync();

            return vendorBids;
        }

        public async Task<DateTime> GetNoticeExpiryDate(Guid procurementPlanId)
        {
            var noticeInformationQuery = await _context.NoticeInformations.Where(x => x.ProcurementPlanId == procurementPlanId).Select(x => x.SubmissionDeadline).SingleOrDefaultAsync();
            return noticeInformationQuery;
        }
        public async Task<ProcurementSummaryDTO> GetProcurementSummary()
        {
            var query = _context.Projects as IQueryable<ProcurementPlan>;

            int total = await query.AsNoTracking().CountAsync();
            int approved = await query.Where(x => x.Status == EProcurementPlanStatus.APPROVED).AsNoTracking().CountAsync();
            int draft = await query.Where(x => x.Status == EProcurementPlanStatus.DRAFT).AsNoTracking().CountAsync();
            int inReview = await query.Where(x => x.Status == EProcurementPlanStatus.INREVIEW).AsNoTracking().CountAsync();

            var summary = new ProcurementSummaryDTO()
            {
                Total = total,
                Approved = approved,
                InReview = inReview,
                Draft = draft,
            };

            return summary;
        }

        public async Task<ProcurementSummaryDTO> GetProcurementSummaryByMinistry(Guid ministryId)
        {
            var query = _context.ProcurementPlans as IQueryable<ProcurementPlan>;
            query = query.Where(p => p.MinistryId == ministryId);

            int total = await query.AsNoTracking().CountAsync();
            int approved = await query.Where(x => x.Status == EProcurementPlanStatus.APPROVED).AsNoTracking()
                                      .CountAsync();
            int draft = await query.Where(x => x.Status == EProcurementPlanStatus.DRAFT).AsNoTracking().CountAsync();
            int inReview = await query.Where(x => x.Status == EProcurementPlanStatus.INREVIEW).AsNoTracking()
                                      .CountAsync();

            var summary = new ProcurementSummaryDTO()
            {
                Total = total,
                Approved = approved,
                InReview = inReview,
                Draft = draft,
            };

            return summary;
        }

        public async Task<PagedList<ProcurementTender>> GetTenderProcurments(ProcurementTenderParameters parameters)
        {
            var query = _context.NoticeInformations as IQueryable<NoticeInformation>;

            var count = query.Count();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim();
                query = query.Where(x => x.ProcurementPlan.Name.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(parameters.Category))
            {
                query = query.Where(x => x.ProcurementPlan.ProcurementCategory.Name == parameters.Category);
            }

            if (parameters.ExpiryDate.HasValue)
            {
                query = query.Where(x => x.SubmissionDeadline.Date == parameters.ExpiryDate.Value.Date);
            }

            if (parameters.MinistryId != null)
            {
                query = query.Where(x => x.ProcurementPlan.MinistryId == parameters.MinistryId);
            }

            var procurementPlan = query.OrderByDescending(x => x.CreateAt).Select(x => new ProcurementTender
            {
                TenderId = x.Id,
                ProcurementPlanId = x.ProcurementPlan.Id,
                PackageNumber = x.ProcurementPlan.PackageNumber,
                Name = x.ProcurementPlan.Name,
                MinistryId = x.ProcurementPlan.MinistryId,
                Description = x.Description,
                ProcurementCategoryId = x.ProcurementPlan.ProcurementCategoryId,
                Category = new ProcurmentCategoryDTO
                {
                    Name = x.ProcurementPlan.ProcurementCategory.Name
                },
                Ministry = new ProcurementMinistryDTO
                {
                    Name = x.ProcurementPlan.Ministry.Name
                },
                OpenDate = x.CreateAt,
                CloseDate = x.SubmissionDeadline

            });

            var procurementResponse =
                await PagedList<ProcurementTender>.Create(procurementPlan, parameters.PageNumber, parameters.PageSize);

            return procurementResponse;
        }

        public async Task<ProcurementTender> GetTender(Guid tenderId)
        {
            var tender = await _context.NoticeInformations.Where(x => x.Id == tenderId).Select(x => new ProcurementTender
            {
                TenderId = x.Id,
                ProcurementPlanId = x.ProcurementPlan.Id,
                PackageNumber = x.ProcurementPlan.PackageNumber,
                Name = x.ProcurementPlan.Name,
                MinistryId = x.ProcurementPlan.MinistryId,
                Description = x.Description,
                EstimatedValueInNaria = x.ProcurementPlan.EstimatedAmountInNaira,
                ProcurementCategoryId = x.ProcurementPlan.ProcurementCategoryId,
                Category = new ProcurmentCategoryDTO
                {
                    Name = x.ProcurementPlan.ProcurementCategory.Name
                },
                Ministry = new ProcurementMinistryDTO
                {
                    Name = x.ProcurementPlan.Ministry.Name
                },
                OpenDate = x.CreateAt,
                CloseDate = x.SubmissionDeadline

            }).SingleOrDefaultAsync();

            return tender;

        }

        public async Task<Contract> GetAcceptedContract(Guid procurementPlanId)
        {
            var contract = await _context.Contracts.Where(x => x.ProcurementPlanId == procurementPlanId && x.Status == EContractStatus.ACCEPTED).SingleOrDefaultAsync();

            return contract;
        }

    }


}
