using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EGPS.Application.Repository
{
    public class DocumentClassRepository : Repository<DocumentClass>, IDocumentClassRepository
    {
        public DocumentClassRepository(EDMSDBContext context)
            : base(context)
        {

        }

        public Task<PagedList<DocumentClass>> GetDocumentClasses(DocumentClassParameters parameters, Guid accountId)
        {
            var query = _context.DocumentClasses as IQueryable<DocumentClass>;
            query = query.Where(x => x.AccountId == accountId).OrderByDescending(d => d.CreateAt);

            var documentClasses = PagedList<DocumentClass>.Create(query, parameters.PageNumber, parameters.PageSize);

            return documentClasses;
        }
    }
}
