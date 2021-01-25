using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IProcurementService
    {
        Task<IEnumerable<ProcurementPlanDocument>> CreateDocument(Guid userId, ProcurementPlanDocumentCreation documents, Guid procurementPlanActivityId);

        bool VerifyDate(DateTime startDate, DateTime endDate);
        Task<IEnumerable<ProcurementPlanDocument>> CreateGenericDocument(GenericProcurementPlanDocumentDto documentDto);
    }
}
