using EGPS.Application.Interfaces;
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
    public class DocumentUploadRepository: Repository<Document>,  IDocumentUploadRepository
    {
        public DocumentUploadRepository(EDMSDBContext context):
            base(context)
        {

        }

        public async Task<IEnumerable<Document>> GetAllDocumentsWithObject(Guid objectId, int objectType)
        {
            var documents =  await _context.Documents.Where(p => p.ObjectId == objectId && p.ObjectType == (EDocumentObjectType)objectType).ToListAsync();

            return documents;
        }
    }
}
