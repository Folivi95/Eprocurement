using EGPS.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EGPS.Application.Services
{
    public class EmailTemplates : IEmailTemplate
    {
        public string GetEmailTemplate(string link, string htmlPath)
        {
            string html;
            if (File.Exists(htmlPath))
            {
                html = File.ReadAllText(htmlPath);
            }
            else
            {
                throw new FormatException("The email template is not provided");
            }
            string msgBody = html.Replace("{email_link}", link);

            return msgBody;
        }
        public string GetDocumentEmailTemplate(string name, string description, DateTime dueDate, string htmlPath)
        {
            string html = string.Empty;
            string msgBody = string.Empty;
            if (File.Exists(htmlPath))
            {
                html = File.ReadAllText(htmlPath);
            }
            else
            {
                throw new FormatException("The email template is not provided");
            }
            msgBody = html.Replace("{doc_name}", name);
            msgBody = msgBody.Replace("{doc_description}", description);
            msgBody = msgBody.Replace("{doc_dueDate}", dueDate.ToString("dd MMMM yyyy"));
            return msgBody;
        }
    }
}
