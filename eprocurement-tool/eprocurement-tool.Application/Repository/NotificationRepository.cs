using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using EGPS.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EGPS.Application.Repository
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        private readonly IUserRepository _userRepository;
        public NotificationRepository(EDMSDBContext context, IUserRepository userRepository)
            : base(context)
        {
            _userRepository = userRepository;
        }

        public Task<PagedList<Notification>> GetNotifications(Guid userId, NotificationParameters parameter)
        {
            var query = _context.Notifications.Where(x => x.NotificationType == Domain.Enums.ENotificationType.Feed).OrderByDescending(x => x.CreateAt) as IQueryable<Notification>;

            query = query.Where(x => x.UserId == userId);
            //check if parameter values are null or empty and add them to query if they aren't
            if (!string.IsNullOrEmpty(parameter.Search))
            {
                string search = parameter.Search.Trim();
                query = query.Where(x => x.Body.ToLower().Contains(search.ToLower()));
            }

            if (parameter.Class.Count > 0)
                query = query.Where(x => parameter.Class.Contains(x.NotificationClass));

            if (parameter.Read.HasValue)
            {
                if (parameter.Read.Value)
                    query = query.Where(x => x.IsRead);
                else if (!parameter.Read.Value)
                    query = query.Where(x => x.IsRead == false);

            }



            var notifications = PagedList<Notification>.Create(query, parameter.PageNumber, parameter.PageSize);

            return notifications;
        }

        public async Task<IEnumerable<Notification>> MarkAsRead(IEnumerable<Guid> NotificationIds)
        {
            List<Notification> NotificationBox = new List<Notification>();

            if (NotificationIds.Count() != 0)
            {
                foreach (var item in NotificationIds)
                {
                    if (item != Guid.Empty)
                    {
                        var query = _context.Notifications.Where(x => x.Id == item && x.IsRead == false).FirstOrDefault();
                        if (query != null)
                        {
                            query.IsRead = true;
                            NotificationBox.Add(query);
                        }
                    }
                }
                _context.Notifications.UpdateRange(NotificationBox);
                await _context.SaveChangesAsync();
            }

            return NotificationBox;
        }


        public async Task LogProcurementNotification(Guid procurementActivityId)
        {
            var procurementActivity = await _context.ProcurementPlanActivities.SingleOrDefaultAsync(p => p.Id == procurementActivityId);
            var procurement = await _context.ProcurementPlans.SingleOrDefaultAsync(p => p.Id == procurementActivity.ProcurementPlanId);


            List<Notification> notifications = new List<Notification>();
            switch (procurementActivity.Title.ToLower())
            {
                case "specific procurement notice":
                    //"Send specific procurement notice notification to all vendors"
                    var allVendors = _context.Users.Where(u => u.UserType == EUserType.VENDOR);

                    //Loop through the vendors to construct notification object
                    foreach (var v in allVendors)
                    {

                        var actionText = "PR-" + procurement.Id.ToString().Substring(0, 4).ToUpper();
                        notifications.Add(
                            new Notification
                            {

                                UserId = v.Id,
                                AccountId = v.AccountId,
                                Body = "The procurement " + actionText + " is opened for bidding",
                                ActionId = procurement.Id,
                                ActionText = actionText,
                                Subject = "Specific Procurement Notice",
                                Recipient = v.Email,
                                Type = Domain.Enums.EType.InApp,
                                NotificationType = Domain.Enums.ENotificationType.Feed,
                                Status = "SUCCESS",
                                NotificationClass = Domain.Enums.ENotificationClass.Special_procurement_notice
                            }
                            );

                    }
                    //"Send specific procurement notice approval notification to procurement officer"

                    var spnProcOfficer = _context.Users.SingleOrDefault(u => u.Id == procurement.CreatedById);
                    var officerActionText = "PR-" + procurement.Id.ToString().Substring(0, 4).ToUpper();
                    notifications.Add(
                        new Notification
                        {

                            UserId = spnProcOfficer.Id,
                            AccountId = spnProcOfficer.AccountId,
                            Body = "Your specific procurement notice " + officerActionText + " has been approved.",
                            ActionId = procurement.Id,
                            ActionText = officerActionText,
                            Subject = "Specific Procurement Notice",
                            Recipient = spnProcOfficer.Email,
                            Type = Domain.Enums.EType.InApp,
                            NotificationType = Domain.Enums.ENotificationType.Feed,
                            Status = "SUCCESS",
                            NotificationClass = Domain.Enums.ENotificationClass.Special_procurement_notice_admin
                        }
                        );
                    break;

                case "bid invitation":
                    //"Send bid invitation notification to vendors who expressed interest"
                    var interestedVendors = _context.VendorBids.Where(p => p.ProcurementPlanId == procurement.Id && p.Type == EVendorContractStatus.INTERESTED).Select(p => p.VendorId);
                    var vendorsDetails = _context.Users.Where(v => interestedVendors.Contains(v.Id));
                    //Loop through the vendors to construct notification object
                    foreach (var v in vendorsDetails)
                    {

                        var actionText = "PR-" + procurement.Id.ToString().Substring(0, 4).ToUpper();
                        notifications.Add(
                            new Notification
                            {
                                UserId = v.Id,
                                AccountId = v.AccountId,
                                Body = "You have been inivited to bid for " + actionText,
                                ActionId = procurement.Id,
                                ActionText = actionText,
                                Subject = "Bid Invitation Notice",
                                Recipient = v.Email,
                                Type = Domain.Enums.EType.InApp,
                                NotificationType = Domain.Enums.ENotificationType.Feed,
                                Status = "SUCCESS",
                                NotificationClass = Domain.Enums.ENotificationClass.Bid_Invitation
                            }
                            );
                        //"Send bid invitation notice approval notification to procurement officer"
                        var binProcOfficer = _context.Users.SingleOrDefault(u => u.Id == procurement.CreatedById);
                        var binofficerActionText = "PR-" + procurement.Id.ToString().Substring(0, 4).ToUpper();
                        notifications.Add(
                            new Notification
                            {

                                UserId = binProcOfficer.Id,
                                AccountId = binProcOfficer.AccountId,
                                Body = "Your bid inivitation notice " + binofficerActionText + " has been approved.",
                                ActionId = procurement.Id,
                                ActionText = binofficerActionText,
                                Subject = "Bid Invitation Notice",
                                Recipient = binProcOfficer.Email,
                                Type = Domain.Enums.EType.InApp,
                                NotificationType = Domain.Enums.ENotificationType.Feed,
                                Status = "SUCCESS",
                                NotificationClass = Domain.Enums.ENotificationClass.Bid_Invitation_admin
                            }
                            );

                    }
                    break;

                case "bid evaluation report and recommendation for awards":
                    //"Send evaluation report  notification to vendors that placed bid "
                    var bidedVendors = _context.VendorBids.Where(p => p.ProcurementPlanId == procurement.Id && p.Type == EVendorContractStatus.NOTSTARTED).Select(p => p.VendorId);
                    var bidedVendorsDetails = _context.Users.Where(v => bidedVendors.Contains(v.Id));
                    foreach (var v in bidedVendorsDetails)
                    {

                        var actionText = "PR-" + procurement.Id.ToString().Substring(0, 4).ToUpper();
                        notifications.Add(
                            new Notification
                            {
                                UserId = v.Id,
                                AccountId = v.AccountId,
                                Body = "Your bid for  " + actionText + " has been evaluated",
                                ActionId = procurement.Id,
                                ActionText = actionText,
                                Subject = "Bid Evaluation Report and Recommendation for Awards",
                                Recipient = v.Email,
                                Type = Domain.Enums.EType.InApp,
                                NotificationType = Domain.Enums.ENotificationType.Feed,
                                Status = "SUCCESS",
                                NotificationClass = Domain.Enums.ENotificationClass.Bid_Evaluation_Report
                            }
                            );

                    }

                    //"Send evaluation report and recommendation approval notification to procurement officer"
                    var benProcOfficer = _context.Users.SingleOrDefault(u => u.Id == procurement.CreatedById);
                    var benofficerActionText = "PR-" + procurement.Id.ToString().Substring(0, 4).ToUpper();
                    notifications.Add(
                        new Notification
                        {

                            UserId = benProcOfficer.Id,
                            AccountId = benProcOfficer.AccountId,
                            Body = "Your bid evaluation report and recommendation for awards " + benofficerActionText + " has been approved.",
                            ActionId = procurement.Id,
                            ActionText = benofficerActionText,
                            Subject = "Bid Evaluation Report and Recommendation for Awards Notice",
                            Recipient = benProcOfficer.Email,
                            Type = Domain.Enums.EType.InApp,
                            NotificationType = Domain.Enums.ENotificationType.Feed,
                            Status = "SUCCESS",
                            NotificationClass = Domain.Enums.ENotificationClass.Bid_Evaluation_Report_admin
                        }
                        );
                    break;

                case "letter of acceptance":
                    //"Send letter of acceptance notification to the recommended vendor -for acceptance"
                    var recommendedVendorsDetails = _context.Users.SingleOrDefault(v => v.Id == _context.VendorBids.SingleOrDefault(b => b.ProcurementPlanId == procurement.Id && b.Type == Domain.Enums.EVendorContractStatus.RECOMMENDED).VendorId);
                    var contract = _context.Contracts.SingleOrDefault(c => c.ProcurementPlanId == procurement.Id && c.ContractorId == recommendedVendorsDetails.Id);
                    if (recommendedVendorsDetails != null)
                    {
                        var actionText = "CON-" + contract.Id.ToString().Substring(0, 4).ToUpper();
                        notifications.Add(
                            new Notification
                            {
                                UserId = recommendedVendorsDetails.Id,
                                AccountId = recommendedVendorsDetails.AccountId,
                                Body = "You have been awarded a letter of acceptance for  " + actionText,
                                ActionId = contract.Id,
                                ActionText = actionText,
                                Subject = "Letter of Acceptance",
                                Recipient = recommendedVendorsDetails.Email,
                                Type = Domain.Enums.EType.InApp,
                                NotificationType = Domain.Enums.ENotificationType.Feed,
                                Status = "SUCCESS",
                                NotificationClass = Domain.Enums.ENotificationClass.Letter_of_Acceptance
                            }
                            );
                    }
                    //"Send letter of acceptance approval notification to procurement officer"
                    var acpProcOfficer = _context.Users.SingleOrDefault(u => u.Id == procurement.CreatedById);
                    var acpofficerActionText = "CON-" + contract.Id.ToString().Substring(0, 4).ToUpper();
                    notifications.Add(
                        new Notification
                        {

                            UserId = acpProcOfficer.Id,
                            AccountId = acpProcOfficer.AccountId,
                            Body = "Your letter of acceptance " + acpofficerActionText + " has been approved.",
                            ActionId = contract.Id,
                            ActionText = acpofficerActionText,
                            Subject = "Letter of Acceptance Approval Notice",
                            Recipient = acpProcOfficer.Email,
                            Type = Domain.Enums.EType.InApp,
                            NotificationType = Domain.Enums.ENotificationType.Feed,
                            Status = "SUCCESS",
                            NotificationClass = Domain.Enums.ENotificationClass.Letter_of_Acceptance_admin
                        }
                        );
                    break;
                case "contract signing":
                    //"Send contract signing notification to the recommended vendor- contractor (for contract signing, upon acceptance of acceptance letter)"
                    var acceptedContract = _context.Contracts.SingleOrDefault(c => c.ProcurementPlanId == procurement.Id && c.Status == EContractStatus.ACCEPTED);
                    if (acceptedContract != null)
                    {
                        var contractor = _context.Users.SingleOrDefault(u => u.Id == acceptedContract.ContractorId);
                        var actionText = "CON-" + acceptedContract.Id.ToString().Substring(0, 4).ToUpper();
                        notifications.Add(
                            new Notification
                            {
                                UserId = contractor.Id,
                                AccountId = contractor.AccountId,
                                Body = "You have been awarded a contract for  " + actionText,
                                ActionId = acceptedContract.Id,
                                ActionText = actionText,
                                Subject = "Contract Signing",
                                Recipient = contractor.Email,
                                Type = Domain.Enums.EType.InApp,
                                NotificationType = Domain.Enums.ENotificationType.Feed,
                                Status = "SUCCESS",
                                NotificationClass = Domain.Enums.ENotificationClass.Contract_Signing
                            }
                            );

                        //"Send contract signing approval notification to procurement officer"
                        var cosProcOfficer = _context.Users.SingleOrDefault(u => u.Id == procurement.CreatedById);
                        var cosofficerActionText = "CON-" + acceptedContract.Id.ToString().Substring(0, 4).ToUpper();
                        notifications.Add(
                            new Notification
                            {

                                UserId = cosProcOfficer.Id,
                                AccountId = cosProcOfficer.AccountId,
                                Body = "Your contract signing " + cosofficerActionText + " has been approved.",
                                ActionId = acceptedContract.Id,
                                ActionText = cosofficerActionText,
                                Subject = "Contract signing Approval Notice",
                                Recipient = cosProcOfficer.Email,
                                Type = Domain.Enums.EType.InApp,
                                NotificationType = Domain.Enums.ENotificationType.Feed,
                                Status = "SUCCESS",
                                NotificationClass = Domain.Enums.ENotificationClass.Contract_Signing_admin
                            }
                            );
                    }
                    break;
                case "publication of contract award":
                    //"Send publication of contract award notification to vendors that placed bid but didn't win"
                    var awardedContract = _context.Contracts.SingleOrDefault(c => c.ProcurementPlanId == procurement.Id && c.Status == EContractStatus.ACCEPTED);
                    if (awardedContract != null)
                    {
                        var contractor = _context.Users.SingleOrDefault(u => u.Id == awardedContract.ContractorId);
                        var loseVendors = _context.VendorBids.Where(p => p.ProcurementPlanId == procurement.Id && p.VendorId != contractor.Id).Select(p => p.VendorId);
                        var loseVendorsDetails = _context.Users.Where(v => loseVendors.Contains(v.Id));
                        foreach (var v in loseVendorsDetails)
                        {

                            var actionText = "PR-" + procurement.Id.ToString().Substring(0, 4).ToUpper();
                            notifications.Add(
                                new Notification
                                {
                                    UserId = v.Id,
                                    AccountId = v.AccountId,
                                    Body = "Contract " + actionText + " has been awarded to someone else",
                                    ActionId = procurement.Id,
                                    ActionText = actionText,
                                    Subject = "Publication of Contract Award",
                                    Recipient = v.Email,
                                    Type = Domain.Enums.EType.InApp,
                                    NotificationType = Domain.Enums.ENotificationType.Feed,
                                    Status = "SUCCESS",
                                    NotificationClass = Domain.Enums.ENotificationClass.Public_Award_Notice
                                }
                                );

                        }
                        //"Send publication of contract award approval notification to procurement officer"
                        var pocProcOfficer = _context.Users.SingleOrDefault(u => u.Id == procurement.CreatedById);
                        var pocofficerActionText = "PR-" + procurement.Id.ToString().Substring(0, 4).ToUpper();
                        notifications.Add(
                            new Notification
                            {

                                UserId = pocProcOfficer.Id,
                                AccountId = pocProcOfficer.AccountId,
                                Body = "Your publication of contract award " + pocofficerActionText + " has been approved.",
                                ActionId = procurement.Id,
                                ActionText = pocofficerActionText,
                                Subject = "Publication of Contract Award Notice",
                                Recipient = pocProcOfficer.Email,
                                Type = Domain.Enums.EType.InApp,
                                NotificationType = Domain.Enums.ENotificationType.Feed,
                                Status = "SUCCESS",
                                NotificationClass = Domain.Enums.ENotificationClass.Public_Award_Notice_admin
                            }
                            );
                    }
                    break;
                default:
                    //do nothing (notifications for custom stage(s))
                    break;
            }
            await _context.Notifications.AddRangeAsync(notifications);
            await _context.SaveChangesAsync();

        }

        public async Task LogAcceptanceLetterResponseNotification(Guid ContractId, EAcceptanceLetterResponse response)
        {

            //Send notification to procuement officer upon contract acceptance or rejection by vendor
            var contract = _context.Contracts.SingleOrDefault(c => c.Id == ContractId);
            var admin = _context.Users.SingleOrDefault(u => u.Id == contract.UserId);
            if (admin != null)
            {


                var actionText = "CON-" + contract.Id.ToString().Substring(0, 4);
                var notice = new Notification
                {
                    UserId = admin.Id,
                    AccountId = admin.AccountId,
                    ActionId = contract.Id,
                    ActionText = actionText,
                    Recipient = admin.Email,
                    Type = Domain.Enums.EType.InApp,
                    NotificationType = Domain.Enums.ENotificationType.Feed,
                    Status = "SUCCESS",
                    Body = "The letter of acceptance for  " + actionText + " was accepted",
                    Subject = "Contract Offer Acceptance",
                    NotificationClass = Domain.Enums.ENotificationClass.Contract_offer_acceptance,

                };
                if (response == EAcceptanceLetterResponse.Reject)
                {
                    notice.Body = "The letter of acceptance for  " + actionText + " was rejected";
                    notice.Subject = "Contract Offer Rejection";
                    notice.NotificationClass = Domain.Enums.ENotificationClass.Contract_offer_rejection;
                }

                _context.Notifications.Add(notice);
                await _context.SaveChangesAsync();
            }
        }

        public async Task LogBidSubmissioNotice(Guid procurementId, Guid vendorId)
        {
            //Send notification to procurement officer about bid submisison
            var vendor = _context.Users.SingleOrDefault(u => u.Id == vendorId);
            var actionText = "VEN-" + vendorId.ToString().Substring(0, 4);
            var procOfficer = _context.Users.SingleOrDefault(u => u.Id == _context.ProcurementPlans.SingleOrDefault(p => p.Id == procurementId).CreatedById);
            var notice = new Notification
            {
                UserId = procOfficer.Id,
                AccountId = procOfficer.AccountId,
                ActionId = vendorId,
                ActionText = actionText,
                Recipient = procOfficer.Email,
                Type = Domain.Enums.EType.InApp,
                NotificationType = Domain.Enums.ENotificationType.Feed,
                Status = "SUCCESS",
                Body = "The vendor " + actionText + " just submitted a bid",
                Subject = "Bid Submission Notice",
                NotificationClass = Domain.Enums.ENotificationClass.Bid_Submission,

            };
            _context.Notifications.Add(notice);
            await _context.SaveChangesAsync();
        }

        public async Task LogContractSigningNotice(Guid procurementId)
        {
            //Send notification to procurement officer about contract signning and to publication of contract award notice
            List<Notification> notifications = new List<Notification>();
            var contract = _context.Contracts.SingleOrDefault(c => c.ProcurementPlanId == procurementId && c.Status == EContractStatus.ACCEPTED);
            var actionText = "CON-" + contract.Id.ToString().Substring(0, 4);
            var procOfficer = _context.Users.SingleOrDefault(u => u.Id == _context.ProcurementPlans.SingleOrDefault(p => p.Id == procurementId).CreatedById);
            notifications.AddRange(new List<Notification> {
                new Notification
                {
                    UserId = procOfficer.Id,
                    AccountId = procOfficer.AccountId,
                    ActionId = contract.Id,
                    ActionText = actionText,
                    Recipient = procOfficer.Email,
                    Type = Domain.Enums.EType.InApp,
                    NotificationType = Domain.Enums.ENotificationType.Feed,
                    Status = "SUCCESS",
                    Body = "The contractor for the contract " + actionText + " just submitted a signed contract",
                    Subject = "Contract Signing Notice",
                    NotificationClass = Domain.Enums.ENotificationClass.Contract_Signing,

                },
                new Notification
                    {
                        UserId = procOfficer.Id,
                        AccountId = procOfficer.AccountId,
                        ActionId = contract.Id,
                        ActionText = actionText,
                        Recipient = procOfficer.Email,
                        Type = Domain.Enums.EType.InApp,
                        NotificationType = Domain.Enums.ENotificationType.Feed,
                        Status = "SUCCESS",
                        Body = "The contract" + actionText + " has been signed, a publication of contract award notice should be sent",
                        Subject = "Publication of Contract Award Notice",
                        NotificationClass = Domain.Enums.ENotificationClass.Public_Award_Notice,

                    }}
                );
            _context.Notifications.AddRange(notifications);
            await _context.SaveChangesAsync();
        }
    }
}
