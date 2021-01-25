using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Services
{
    public class ProcurementService : IProcurementService
    {
        private readonly IPhotoAcessor _photoAcessor;
        private readonly IProcurementPlanRepository _procurementPlanRepository;

        public ProcurementService(IPhotoAcessor photoAcessor, IProcurementPlanRepository procurementPlanRepository)
        {
            _photoAcessor = photoAcessor;
            _procurementPlanRepository = procurementPlanRepository;
        }
        public async Task<IEnumerable<ProcurementPlanDocument>> CreateDocument(Guid userId, ProcurementPlanDocumentCreation documents, Guid procurementPlanActivityId)
        {
            var procurementPlanDocuments = new List<ProcurementPlanDocument>();
            if (documents != null)
            {
                var documentsExists = await _procurementPlanRepository.IsProcurementPlanActivityDocumentExists(procurementPlanActivityId);

                if (!documentsExists)
                {
                    _procurementPlanRepository.UpdateProcurementPlanActivityRevisedDate(procurementPlanActivityId);
                }

                // _procurementPlanRepository.RemoveProcurementPlanDocument(documents.RemovedDocuments);

                if (documents.RemovedDocuments != null && documents.RemovedDocuments.Count > 0)
                {
                    foreach (var document in documents.RemovedDocuments)
                    {
                        if (document != null)
                        {
                            var doc = await _procurementPlanRepository.GetProcurementPlanActivityDocument(document.Value);
                            if(doc != null)
                                doc.Deleted = true;
                        }
                    }
                }

                var mandatoryDocuments = documents.MandatoryDocument;

                if (mandatoryDocuments != null && mandatoryDocuments.Count > 0)
                {
                    foreach (var document in mandatoryDocuments)
                    {
                        var fileUploadResponse = _photoAcessor.AddFile(document);
                        var file = JsonConvert.SerializeObject(fileUploadResponse);
                        var procurmentPlanDocument = new ProcurementPlanDocument();
                        if (documents.ObjectType.HasValue)
                        {
                            procurmentPlanDocument.File = file;
                            procurmentPlanDocument.Name = document.FileName;
                            procurmentPlanDocument.DocumentStatus = EDocumentStatus.MANDATORY;
                            procurmentPlanDocument.CreatedById = userId;
                            procurmentPlanDocument.ObjectId = procurementPlanActivityId;
                            procurmentPlanDocument.ObjectType = documents.ObjectType.Value;
                        }
                        else
                        {
                            procurmentPlanDocument.File = file;
                            procurmentPlanDocument.Name = document.FileName;
                            procurmentPlanDocument.DocumentStatus = EDocumentStatus.MANDATORY;
                            procurmentPlanDocument.CreatedById = userId;
                            procurmentPlanDocument.ObjectId = procurementPlanActivityId;
                        }
                        
                        procurementPlanDocuments.Add(procurmentPlanDocument);
                    }
                }

                var supportingDocuments = documents.SupportingDocument;
                if (supportingDocuments != null && supportingDocuments.Count > 0)
                {
                    foreach (var document in supportingDocuments)
                    {
                        var fileUploadResponse = _photoAcessor.AddPhoto(document);
                        var file = JsonConvert.SerializeObject(fileUploadResponse);

                        var procurmentPlanDocument = new ProcurementPlanDocument();
                        if (documents.ObjectType.HasValue)
                        {
                            procurmentPlanDocument.File = file;
                            procurmentPlanDocument.Name = document.FileName;
                            procurmentPlanDocument.DocumentStatus = EDocumentStatus.SUPPORTING;
                            procurmentPlanDocument.CreatedById = userId;
                            procurmentPlanDocument.ObjectId = procurementPlanActivityId;
                            procurmentPlanDocument.ObjectType = documents.ObjectType.Value;
                        }
                        else
                        {
                            procurmentPlanDocument.File = file;
                            procurmentPlanDocument.Name = document.FileName;
                            procurmentPlanDocument.DocumentStatus = EDocumentStatus.SUPPORTING;
                            procurmentPlanDocument.CreatedById = userId;
                            procurmentPlanDocument.ObjectId = procurementPlanActivityId;
                        }
                        procurementPlanDocuments.Add(procurmentPlanDocument);
                    }
                }
            }


            return procurementPlanDocuments;

        }

        public async Task<IEnumerable<ProcurementPlanDocument>> CreateGenericDocument(GenericProcurementPlanDocumentDto documentDto)
        {
            var procurementPlanDocuments = new List<ProcurementPlanDocument>();

            foreach (var document in documentDto.Documents)
            {
                var fileUploadResponse = _photoAcessor.AddFile(document);
                var file = JsonConvert.SerializeObject(fileUploadResponse);

                var procurementPlanDocument = new ProcurementPlanDocument
                {
                    File = file,
                    Name = document.FileName,
                    DocumentStatus = documentDto.Status,
                    CreatedById = documentDto.UserId,
                    ObjectId = documentDto.ObjectId,
                    ObjectType = documentDto.ObjectType
                };
                procurementPlanDocuments.Add(procurementPlanDocument);
            }
            return procurementPlanDocuments;
        }

        public bool VerifyDate(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                return false;
            }

            return true;
        }
    }
}
