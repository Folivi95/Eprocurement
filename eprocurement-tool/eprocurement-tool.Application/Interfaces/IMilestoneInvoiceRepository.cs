using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IMilestoneInvoiceRepository : IRepository<MilestoneInvoice>
    {
        Task<PagedList<MilestoneInvoice>> GetInvoiceForProject(Guid projectId, ResourceParameters parameter);
        Task<ProjectMileStone> GetTransactionsByMilestoneId(Guid mileStoneId);
        Task<PagedList<MilestoneInvoice>> GetMilestoneInvoices(Expression<Func<MilestoneInvoice, bool>> expression, TransactionParameters parameters);

        Task<PagedList<ProjectMileStone>> GetPaidInvoiceMileStoneTaskFromProject(Guid projectId,
            TransactionParameters parameters);

        Task<PagedList<MilestoneInvoice>> GetMilestoneInvoicesByVendor(TransactionParameters parameters, Guid vendorId);
     
    }
}
