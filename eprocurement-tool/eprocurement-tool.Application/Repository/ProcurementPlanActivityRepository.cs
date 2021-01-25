using EGPS.Application.Interfaces;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;

namespace EGPS.Application.Repository
{
    public class ProcurementPlanActivityRepository: Repository<ProcurementPlanActivity>, IProcurementPlanActivityRepository
    {
        public ProcurementPlanActivityRepository(EDMSDBContext context):
            base(context)
        {

        }
    }
}
