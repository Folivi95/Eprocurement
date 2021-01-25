using EGPS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IDocumentUploadRepository: IRepository<Document>
    {
        Task<IEnumerable<Document>> GetAllDocumentsWithObject(Guid objectId, int objectType);
    }
}
