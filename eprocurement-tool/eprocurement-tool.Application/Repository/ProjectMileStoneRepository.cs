using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGPS.Domain.Enums;

namespace EGPS.Application.Repository
{
    public class ProjectMileStoneRepository : Repository<ProjectMileStone>, IProjectMileStoneRepository
    {
        public ProjectMileStoneRepository(EDMSDBContext context) : base(context)
        {

        }

        public async Task<bool> CreateMilestoneInvoice(ProjectMileStone milestone, MilestoneInvoiceForCreation milestoneInvoice)
        {
            string invoiceNumber = "";
            string invPrefix = "INV-";
            decimal price = 0m;
            string invoiceName = $"Invoice for {milestone.Title}";
            string invoiceDescription = $"Invoice for {milestone.Description}";
            int invoiceCount = await _context.MilestoneInvoices.CountAsync();
            StringBuilder sb = new StringBuilder();

            //generate invoice number
            sb.Append(invPrefix);
            sb.Append((++invoiceCount).ToString("D4"));
            invoiceNumber = sb.ToString();

            //calculate price
            if (milestone.MilestoneTasks == null)
            {
                price = 0m;     //set price to zero
            }
            else
            {
                foreach (var item in milestone.MilestoneTasks)
                {
                    price += (decimal)item.EstimatedValue;
                }
            }

            MilestoneInvoice newMilestoneInvoice = new MilestoneInvoice()
            {
                Name = invoiceName,
                Description = invoiceDescription,
                Price = price,
                DueDate = milestoneInvoice.DueDate,
                InvoiceNumber = invoiceNumber,
                ProjectMileStoneId = milestone.Id
            };

            //add new milestone invoice to database
            try
            {
                var invoiceExists = await MilestoneInvoiceExists(milestone.Id);

                if (invoiceExists != null)
                {
                    invoiceExists.Name = invoiceName;
                    invoiceExists.Description = invoiceDescription;
                    invoiceExists.Price = price;
                    invoiceExists.DueDate = milestoneInvoice.DueDate;
                    invoiceExists.InvoiceNumber = invoiceNumber;

                    _context.MilestoneInvoices.Update(invoiceExists);
                }
                else
                {
                    await _context.MilestoneInvoices.AddAsync(newMilestoneInvoice);
                }
               
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<MilestoneInvoice> MilestoneInvoiceExists(Guid milestoneId)
        {
            var milestone = await _context.MilestoneInvoices.Where(b => b.ProjectMileStoneId == milestoneId).AsNoTracking().FirstOrDefaultAsync();

            return milestone;
        }

        public async Task<MilestoneInvoice> GetMilestoneInvoice(Guid milestoneId)
        {
            var query = _context.MilestoneInvoices.Where(x => x.ProjectMileStoneId == milestoneId)
                        .Include(x => x.ProjectMileStone).ThenInclude(m => m.MilestoneTasks).AsNoTracking();

            var milestoneInvoice = await query.FirstOrDefaultAsync();

            return milestoneInvoice;
        }

        public async Task<decimal> GetPercentageComplete(Guid projectMileStoneId)
        {
            var totalTask =(decimal) (await _context.MilestoneTasks
                .LongCountAsync(a => a.MileStoneId == projectMileStoneId));

            var completedTask = (decimal) (await _context.MilestoneTasks
                .LongCountAsync(a => a.MileStoneId == projectMileStoneId && a.Status == EMilestoneTaskStatus.DONE));

            totalTask = (totalTask == 0 ? 1 : totalTask);
            var percentage = (completedTask / totalTask) * 100;
            return percentage;
          
        }

        public async Task<IEnumerable<MilestoneTask>> GetMilestoneTasks(Guid mileStoneId)
        {
            var milestoneTasks = await _context.MilestoneTasks.Where(m => m.MileStoneId == mileStoneId).ToListAsync();

            return milestoneTasks;
        }
    }
}
