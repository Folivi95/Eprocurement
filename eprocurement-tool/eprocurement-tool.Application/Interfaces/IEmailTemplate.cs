using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Interfaces
{
    public interface IEmailTemplate
    {
        string GetEmailTemplate(string link, string htmlTemplate);
        string GetDocumentEmailTemplate(string name, string description, DateTime dueDate, string htmlTemplate);
    }
}
