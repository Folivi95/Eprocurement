using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IDocumentClassRepository : IRepository<DocumentClass>
    {
        Task<PagedList<DocumentClass>> GetDocumentClasses(DocumentClassParameters parameters, Guid accountId);
    }
}
