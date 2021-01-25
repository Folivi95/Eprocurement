using EGPS.Application.Interfaces;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using System;
using System.Linq;
using System.Threading.Tasks;
using EGPS.Application.Helpers;
using EGPS.Application.Models;
using EGPS.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EGPS.Application.Repository
{
    public class GeneralPlanRepository: Repository<GeneralPlan>, IGeneralPlanRepository
    {
        public GeneralPlanRepository(EDMSDBContext context) : base(context)
        {

        }

        public async Task<GeneralPlan> GetGeneralPlanById(Guid generalPlanId)
        {
            var generalPlan =await _context.GeneralPlans.Where(x => x.Id == generalPlanId).Include(x => x.Ministry).SingleOrDefaultAsync();

            return generalPlan;
        }

        public async Task<PagedList<GeneralPlanResponse>> GetGeneralPlan(GeneralPlanParameters parameters)
        {

            var query = _context.GeneralPlans as IQueryable<GeneralPlan>;

            var c = await query.CountAsync(x => x.Status == EGeneralPlanStatus.PENDING);

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim();
                query = query.Where(x => x.Description.Contains(search));
            }

            if (parameters.MinistryId != null)
            {
                query = query.Where(x => x.MinistryId == parameters.MinistryId);
            }

            if (parameters.Year.HasValue)
            {
                query = query.Where(x => x.CreateAt.Year == parameters.Year.Value);
            }

            if (!string.IsNullOrWhiteSpace(parameters.Status) &&
                Enum.IsDefined(typeof(EGeneralPlanStatus), parameters.Status.ToUpper()))
            {
                var enumType = parameters.Status.ParseStringToEnum(typeof(EGeneralPlanStatus));
                query = query.Where(x => x.Status == (EGeneralPlanStatus) enumType);
            }

            var generalPlan = query.OrderByDescending(x => x.CreateAt).Select(x => new GeneralPlanResponse
            {
                Id          = x.Id,
                Name        = x.Name,
                Description = x.Description,
                Title       = x.Title,
                Country     = x.Country,
                PhoneNumber = x.PhoneNumber,
                Email       = x.Email,
                Address     = x.Address,
                Fax         = x.Fax,
                Website     = x.Website,
                MinistryId  = x.MinistryId,
                CreatedById = x.CreatedById,
                CreatedAt   = x.CreateAt,
                UpdatedAt   = x.UpdatedAt,
                Ministry = new MinistryPlan
                {
                    Name = x.Ministry.Name,
                    Code = x.Ministry.Code
                },
                Categories = x.ProcurementPlans.Count(),
                Amount     = x.ProcurementPlans.Sum(x => x.EstimatedAmountInNaira),
                Status     = x.Status
            });

            var generalPlanResponse = await PagedList<GeneralPlanResponse>.Create(generalPlan, parameters.PageNumber, parameters.PageSize);

            return generalPlanResponse;
        }

        public async Task<PagedList<ProcurementsResponse>>GetProcurments(ProcurementPlanParameters parameters, Guid generalPlanId)
        {
            var query = _context.ProcurementPlans.Where(x => x.GeneralPlanId == generalPlanId);

            if (parameters.MinistryId != null)
            {
                query = query.Where(x => x.MinistryId == parameters.MinistryId);
            }

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim();
                query = query.Where(x => x.Name.Contains(search) || x.PackageNumber.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(parameters.Status) &&
                Enum.IsDefined(typeof(EProcurementPlanStatus), parameters.Status.ToUpper()))
            {
                var enumType = parameters.Status.ParseStringToEnum(typeof(EProcurementPlanStatus));
                query = query.Where(x => x.Status == (EProcurementPlanStatus) enumType);
            }

            if (!string.IsNullOrWhiteSpace(parameters.Stage) &&
                Enum.IsDefined(typeof(EProcurementStage), parameters.Stage.ToUpper()))
            {
                var enumType = parameters.Stage.ParseStringToEnum(typeof(EProcurementStage));
                query = query.Where(x => x.Stage == (EProcurementStage) enumType);
            }

            if (!string.IsNullOrWhiteSpace(parameters.Category))
            {
                var category = parameters.Category.Trim();
                query = query.Where(x => x.ProcurementCategory.Name == category);
            }

            var procurementPlan = query.OrderByDescending(x => x.CreateAt).Select(x => new ProcurementsResponse
            {
                Id = x.Id,
                Name = x.Name,
                ProcurementCategoryId = x.ProcurementCategoryId,
                ProcessTypeId = x.ProcessTypeId,
                ProcurementMethodId = x.ProcurementMethodId,
                EstimatedAmountInNaira = x.EstimatedAmountInNaira,
                EstimatedAmountInDollars = x.EstimatedAmountInDollars,
                QualificationMethodId = x.QualificationMethodId,
                ReviewMethodId = x.ReviewMethodId,
                MinistryId = x.MinistryId,
                Description = x.Description,
                PackageNumber = x.PackageNumber,
                GeneralPlanId = x.GeneralPlanId,
                Status = x.Status,
                Stage = x.Stage,
                CreatedAt = x.CreateAt,
                UpdatedAt = x.UpdatedAt,
                Method = new ProcurmentMethodDTO
                {
                    Name = x.ProcurementMethod.Name,
                    Code = x.ProcurementMethod.Code
                },
                Category = new ProcurmentCategoryDTO { Name = x.ProcurementCategory.Name}
            });

            var procurementResponse =
                await PagedList<ProcurementsResponse>.Create(procurementPlan, parameters.PageNumber, parameters.PageSize);

            return procurementResponse;
        }

        public async Task<GeneralPlanSummaryDto> GetGeneralPlanSummary(ProcurementPlanParameters parameters)
        {
            var query = _context.GeneralPlans as IQueryable<GeneralPlan>;

            if (parameters.MinistryId != null)
            {
                query = query.Where(x => x.MinistryId == parameters.MinistryId);
            }

            var totalGeneralPlan= await query.CountAsync();
            var totalPending = await query.Where(x => x.Status == EGeneralPlanStatus.PENDING).CountAsync();
            var totalApproved = await query.Where(x => x.Status == EGeneralPlanStatus.APPROVED).CountAsync();

            var summaryDetails = new GeneralPlanSummaryDto()
            {
                Total = totalGeneralPlan,
                ApprovedTotal = totalApproved,
                PendingTotal = totalPending
            };

            return summaryDetails;
        }

        public async Task<ProcurementPlanSummaryDto> GetprocurementPlanSummary(Guid generalPlan)
        {
            var query = _context.ProcurementPlans.Where(x => x.GeneralPlanId == generalPlan)
                                .GroupBy(x => new {x.ProcurementCategory.Name, x.Status})
                                .Select(x => new ProcurementGroup
                                {
                                    Category = x.Key.Name,
                                    Status = x.Key.Status,
                                    Count = x.Count()
                                });

            var result = await query.ToListAsync();

            var goods = "Goods";
            var works = "Works";
            var consultation = "Consultation";
            var nonConsultation = "Non Consultation";

            var summary = new ProcurementPlanSummaryDto
            {
                Consultancy = new ConsultancyCategory
                {
                    Incomplete = result.GetCount(consultation, EProcurementPlanStatus.INREVIEW),
                    Approved = result.GetCount(consultation, EProcurementPlanStatus.APPROVED),
                    Total = result.GetTotalCount(consultation)
                },
                Goods = new GoodCategory
                {
                    Incomplete = result.GetCount(goods, EProcurementPlanStatus.INREVIEW),
                    Approved = result.GetCount(goods, EProcurementPlanStatus.APPROVED),
                    Total = result.GetTotalCount(goods)
                },
                NonConsultancy = new NonConsultancyCategory
                {
                    Incomplete = result.GetCount(nonConsultation, EProcurementPlanStatus.INREVIEW),
                    Approved = result.GetCount(nonConsultation, EProcurementPlanStatus.APPROVED),
                    Total = result.GetTotalCount(nonConsultation)
                },
                Works = new WorkCategory
                {
                    Incomplete = result.GetCount(works, EProcurementPlanStatus.INREVIEW),
                    Approved = result.GetCount(works, EProcurementPlanStatus.APPROVED),
                    Total = result.GetTotalCount(works)
                }
            };

            return summary;
        }

    }
}
