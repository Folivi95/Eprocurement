using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace EGPS.Application.Services
{
    public class DocumentUploadService : IDocumentUploadService
    {
        private readonly IPhotoAcessor _photoAcessor;


        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="photoAccessor"></param>
        public DocumentUploadService(IPhotoAcessor photoAcessor)
        {
            _photoAcessor = photoAcessor ?? throw new ArgumentNullException(nameof(photoAcessor));
        }

        public IEnumerable<Document> CreateGenericDocument(GenericProcurementPlanDocumentDto documentDto)
        {
            var documents = new List<Document>();

            foreach (var document in documentDto.Documents)
            {
                var fileUploadResponse = _photoAcessor.AddFile(document);
                var file = JsonConvert.SerializeObject(fileUploadResponse);

                var newDocument = new Document
                {
                    File = file,
                    Name = document.FileName,
                    DocumentStatus = documentDto.Status,
                    CreatedById = documentDto.UserId,
                    ObjectId = documentDto.ObjectId,
                    ObjectType = documentDto.ObjectType
                };
                documents.Add(newDocument);
            }
            return documents;
        }
    }
}
