using CloudinaryDotNet.Actions;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EGPS.Application.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IEmailLogger _emailLogger;
        private readonly int maxNumSent = 5;
        private readonly IDictionary<string, EmailResponse> emailResponses = new Dictionary<string, EmailResponse>();
        public EmailSender(IEmailLogger emailLogger, IConfiguration configuration)
        {
            _emailLogger = emailLogger ?? throw new ArgumentNullException(nameof(emailLogger));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public async Task SendEmailAsync(string subject, string body, string recipient, Notification notification)
        {
            var count = 0;

            while (count < maxNumSent)
            {
                var responseStatusCode = await ExecuteAsync(Configuration["SENDGRID_API_KEY"], subject, body, recipient);

                if(responseStatusCode >= 400 && responseStatusCode <= 500)
                {
                    count++;
                    continue;
                }
                else
                {
                    break;
                }
            }

            if(count == maxNumSent)
            {
                notification.Status = "FAILED";
            }

            await _emailLogger.LogEmailAsync(notification);
        }

        public async Task<int> ExecuteAsync(string apiKey, string subject, string body, string recipient)
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreply@egps.com", "EGPS Cooperations");
            var to = new EmailAddress(recipient);
            var plainText = "";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainText, body);

            var response = await client.SendEmailAsync(msg);
            return (int)response.StatusCode;
        }

        public async Task<List<string>> SendMultipleEmailsAsync(string subject, IDictionary<string, string> body, IList<Notification> notifications)
        {
            var failedEmails = new List<string>();
            await ExecuteMultipleEmailsAsync(Configuration["SENDGRID_API_KEY"], subject, body);

            foreach (KeyValuePair<string, EmailResponse> item in emailResponses)
            {
                if (item.Value.response >= 400)
                {
                    notifications.Where(x => x.Recipient == item.Key)
                        .Select(c =>
                        {
                            c.Status = "FAILED";

                            return c;
                        }).ToList();
                    failedEmails.Add(item.Key);
                }
            }


            await _emailLogger.LogMultipleEmailAsync(notifications);

            return failedEmails;
        }

        public async Task ExecuteMultipleEmailsAsync(string apiKey, string subject, IDictionary<string, string> body)
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreply@edms.com", "EGPS Cooperations");
            var plainText = string.Empty;
            var htmlContent = body;

            foreach (KeyValuePair<string, string> item in body)
            {
                var to = new EmailAddress(item.Key);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainText, item.Value);

                Response response = await client.SendEmailAsync(msg);

                emailResponses.Add(item.Key, new EmailResponse
                {
                    response = (int)response.StatusCode,
                    body = item.Value
                });
            }
        }
    }
}
