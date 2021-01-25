using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EGPS.Application.Repository
{
    public class MilestoneInvoiceRepository : Repository<MilestoneInvoice>, IMilestoneInvoiceRepository
    {
        public MilestoneInvoiceRepository(EDMSDBContext context) : base(context)
        {

        }

        public  Task<PagedList<MilestoneInvoice>> GetMilestoneInvoices(Expression<Func<MilestoneInvoice, bool>> expression, TransactionParameters parameters)
        {
            var query = _context.MilestoneInvoices.Where(expression).Include(x => x.ProjectMileStone)  as IQueryable<MilestoneInvoice>;

            if (!String.IsNullOrEmpty(parameters.Search))
                query = query.Where(x => x.Description.ToLower().Contains(parameters.Search.ToLower()));

            if (parameters.StartDate.HasValue && parameters.EndDate.HasValue)
                query = query.Where(x => x.CreateAt.Date >= parameters.StartDate.Value.Date && x.CreateAt.Date <= parameters.EndDate.Value);


            var invoices = PagedList<MilestoneInvoice>.Create(query, parameters.PageNumber, parameters.PageSize);

            return invoices;
        }

        public async  Task<PagedList<MilestoneInvoice>> GetInvoiceForProject(Guid projectId, ResourceParameters parameter)
        {
            var query  = this.Query(a => a.ProjectMileStone.ProjectId == projectId)
                .Include(a => a.ProjectMileStone)
                .OrderByDescending(a => a.CreateAt);  

            var invoices =  await PagedList<MilestoneInvoice>.Create(query, parameter.PageNumber, parameter.PageSize);
            return invoices;
        }

        public async Task<ProjectMileStone> GetTransactionsByMilestoneId(Guid mileStoneId)
        {
            return await _context.ProjectMileStones
                .Include(x => x.Project)
                .Include(x => x.MilestoneInvoice)
                .SingleOrDefaultAsync(x => x.Id == mileStoneId);
        }

        public async Task<PagedList<ProjectMileStone>> GetPaidInvoiceMileStoneTaskFromProject(Guid projectId, TransactionParameters parameters)
        {
            var query =  _context.ProjectMileStones
                .Where(a => a.ProjectId == projectId && a.MilestoneInvoice.Status == parameters.Status)
                .Include(a => a.MilestoneInvoice)
                .Include(a => a.Project);
            var invoices = await PagedList<ProjectMileStone>.Create(query, parameters.PageNumber, parameters.PageSize);
            return invoices;
        }

        public async  Task<PagedList<MilestoneInvoice>> GetMilestoneInvoicesByVendor(TransactionParameters parameters, Guid vendorId)
        {
            var query = _context.MilestoneInvoices.Include(x => x.ProjectMileStone)  as IQueryable<MilestoneInvoice>;

            if (!String.IsNullOrEmpty(parameters.Search))
                query = query.Where(x => x.UniqueId.Trim()
                    .Equals(parameters.Search.Trim())
                );
            
            if (parameters.StartDate.HasValue && parameters.EndDate.HasValue)
                query = query.Where(x => x.CreateAt.Date >= parameters.StartDate.Value.Date && x.CreateAt.Date <= parameters.EndDate.Value);


            query = query.Where(x => x.ProjectMileStone.CreatedById == vendorId);


            var invoices = await PagedList<MilestoneInvoice>.Create(query, parameters.PageNumber, parameters.PageSize);

            return invoices;
        }
    }
}
