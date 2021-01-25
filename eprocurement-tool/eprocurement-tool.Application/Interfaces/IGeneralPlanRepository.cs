using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IGeneralPlanRepository: IRepository<GeneralPlan>
    {
        Task<GeneralPlan> GetGeneralPlanById(Guid generalPlanId);
        Task<PagedList<GeneralPlanResponse>> GetGeneralPlan(GeneralPlanParameters parameters);
        Task<PagedList<ProcurementsResponse>> GetProcurments(ProcurementPlanParameters parameters, Guid generalPlanId);
        Task<GeneralPlanSummaryDto> GetGeneralPlanSummary(ProcurementPlanParameters procurementParameter);
        Task<ProcurementPlanSummaryDto> GetprocurementPlanSummary(Guid generalPlan);
    }
}
