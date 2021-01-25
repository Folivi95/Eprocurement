using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EGPS.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task Seed(EDMSDBContext context)
        {
            var userId = context.Users.FirstOrDefault().Id;
            var accountId = context.Users.FirstOrDefault().AccountId;

            // seed database with Resources
            if (!context.Resources.Any())
            {
                var resources = new Resource[]
                {
                    new Resource{Name = "Set up company profile"},
                    new Resource{Name = "Users"},
                    new Resource{Name = "Custom role creation"},
                    new Resource{Name = "Departments"},
                    new Resource{Name = "Units"},
                    new Resource{Name = "Documents overview - Department"},
                    new Resource{Name = "Documents overview - Unit"},
                    new Resource{Name = "Document classes"},
                    new Resource{Name = "Workflows"},
                    new Resource{Name = "Audit log"},
                    new Resource{Name = "Archive"},
                    new Resource{Name = "Folders in archive"},
                    new Resource{Name = "Sub-folders in archive"},
                };

                await context.Resources.AddRangeAsync(resources);
            }


            //seed States Table with data
            if (!context.States.Any())
            {
                var states = new State[]
            {
                new State
                {
                    CountryId= 1,
                    Code= "NG-FC",
                    Name= "Abuja Federal Capital Territory"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-AB",
                    Name= "Abia"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-AD",
                    Name= "Adamawa"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-AK",
                    Name= "Akwa Ibom"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-AN",
                    Name= "Anambra"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-BA",
                    Name= "Bauchi"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-BY",
                    Name= "Bayelsa"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-BE",
                    Name= "Benue"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-BO",
                    Name= "Borno"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-CR",
                    Name= "Cross River"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-DE",
                    Name= "Delta"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-EB",
                    Name= "Ebonyi"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-ED",
                    Name= "Edo"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-EK",
                    Name= "Ekiti"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-EN",
                    Name= "Enugu"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-GO",
                    Name= "Gombe"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-IM",
                    Name= "Imo"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-JI",
                    Name= "Jigawa"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-KD",
                    Name= "Kaduna"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-KN",
                    Name= "Kano"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-KT",
                    Name= "Katsina"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-KE",
                    Name= "Kebbi"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-KO",
                    Name= "Kogi"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-KW",
                    Name= "Kwara"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-LA",
                    Name= "Lagos"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-NA",
                    Name= "Nasarawa"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-NI",
                    Name= "Niger"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-OG",
                    Name= "Ogun"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-ON",
                    Name= "Ondo"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-OS",
                    Name= "Osun"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-OY",
                    Name= "Oyo"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-PL",
                    Name= "Plateau"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-RI",
                    Name= "Rivers"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-SO",
                    Name= "Sokoto"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-TA",
                    Name= "Taraba"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-YO",
                    Name= "Yobe"
                },
                new State
                {
                    CountryId= 1,
                    Code= "NG-ZA",
                    Name= "Zamfara"
                }
            };

                await context.States.AddRangeAsync(states);     // add states to States table in database
            }

            
            if (!context.Countries.Any())
            {
                //Seed Country data.
                var country = new Country { Code = "NGN", Name = "Nigeria" };

                //Add seeded country data.
                await context.Countries.AddAsync(country);
            }

            if (!context.Accounts.Any())
            {
                //Seed Account data.
                var Account = new Account { CompanyName = "Sample State Government" };

                await context.Accounts.AddAsync(Account);
            }

            if (!context.BusinessCategories.Any())
            {
                var businessCategories = new BusinessCategory[]
                {
                    new BusinessCategory()
                    {
                        Title = "Electrical",
                        Description = "Electrical business category",
                        CreatedByID = userId
                    },

                    new BusinessCategory()
                    {
                        Title = "Mechanical plumbing",
                        Description = "mechanical plumbing business category",
                        CreatedByID = userId
                    },

                    new BusinessCategory()
                    {
                        Title = "Highrise buildings",
                        Description = "Highrise buildings business category",
                        CreatedByID = userId
                    },

                    new BusinessCategory()
                    {
                        Title = "Hospitals",
                        Description = "Hospitals business category",
                        CreatedByID = userId
                    },
                };

                await context.BusinessCategories.AddRangeAsync(businessCategories);
            }

            if (!context.BusinessServices.Any())
            {
                var businessCategoryId = context.BusinessCategories.FirstOrDefault().Id;
                var businessServices = new BusinessService[]
                {
                    new BusinessService()
                    {
                        Title = "Industrials/Factorial Buildings",
                        Description = "For people into building industrial and factorial buildings",
                        BusinessCategoryID = businessCategoryId,
                        CreatedByID = userId
                    },

                    new BusinessService()
                    {
                        Title = "Electrical",
                        Description = "For people into building industrial and factorial buildings",
                        BusinessCategoryID = businessCategoryId,
                        CreatedByID = userId
                    },

                    new BusinessService()
                    {
                        Title = "High rise buildings",
                        Description = "For people into building industrial and factorial buildings",
                        BusinessCategoryID = businessCategoryId,
                        CreatedByID = userId
                    },

                    new BusinessService()
                    {
                        Title = "Agriculture Facility",
                        Description = "For people into building industrial and factorial buildings",
                        BusinessCategoryID = businessCategoryId,
                        CreatedByID = userId
                    },

                    new BusinessService()
                    {
                        Title = "Agriculture Facility",
                        Description = "For people into building industrial and factorial buildings",
                        BusinessCategoryID = businessCategoryId,
                        CreatedByID = userId
                    },

                    new BusinessService()
                    {
                        Title = "High rise buildings",
                        Description = "For people into building industrial and factorial buildings",
                        BusinessCategoryID = businessCategoryId,
                        CreatedByID = userId
                    },

                    new BusinessService()
                    {
                        Title = "Electrical",
                        Description = "For people into building industrial and factorial buildings",
                        BusinessCategoryID = businessCategoryId,
                        CreatedByID = userId
                    },

                    new BusinessService()
                    {
                        Title = "Industrials/Factorial Buildings",
                        Description = "For people into building industrial and factorial buildings",
                        BusinessCategoryID = businessCategoryId,
                        CreatedByID = userId
                    }
                };

                await context.BusinessServices.AddRangeAsync(businessServices);
            }

            if (!context.VendorDocumentTypes.Any())
            {
                var vendorDocumentTypes = new VendorDocumentType[]
                {
                    new VendorDocumentType()
                    {
                        Title = "Company Letter Head",
                        CreatedById = userId
                    },
                    new VendorDocumentType()
                    {
                        Title = "Certificate Of IncorPoration/ Registration of Business Name",
                        CreatedById = userId
                    },
                    new VendorDocumentType()
                    {
                        Title = "CAC Form (StateMent Of Share Capital)",
                        CreatedById = userId
                    },
                    new VendorDocumentType()
                    {
                        Title = "CAC Form (Particulars of First Directors)",
                        CreatedById = userId
                    },
                    new VendorDocumentType()
                    {
                        Title = "Copy Of Company Memorandum",
                        CreatedById = userId
                    }
                };

                await context.VendorDocumentTypes.AddRangeAsync(vendorDocumentTypes);
            }


            if (!context.RegistrationPlans.Any())
            {
                var registrationPlans = new RegistrationPlan[]
                {
                    new RegistrationPlan()
                    {
                        Title = "Class A",
                        Description = "Product G Registration Fee For Contract Value",
                        ContractMinValue = 10000000,
                        ContractMaxValue = 500000,
                        Grade = "Class A",
                        Fee = 25000,
                        TenureInDays = 365,
                        CreatedBy = userId,
                        RegistrationCategoryType = ERegistrationCategoryType.FIRST_TIME_VENDOR,
                    },
                    new RegistrationPlan()
                    {
                        Title = "Class B",
                        Description = "Product G Registration Fee For Contract Value",
                        ContractMinValue = 100000000,
                        ContractMaxValue = 1000000000,
                        Grade = "Class B",
                        Fee = 150000,
                        TenureInDays = 365,
                        CreatedBy = userId,
                        RegistrationCategoryType = ERegistrationCategoryType.FIRST_TIME_VENDOR,
                    },
                    new RegistrationPlan()
                    {
                        Title = "Class C",
                        Description = "Product G Registration Fee For Contract Value",
                        ContractMinValue = 1000000000,
                        ContractMaxValue = 2500000000,
                        Grade = "Class C",
                        Fee = 250000,
                        TenureInDays = 365,
                        CreatedBy = userId,
                        RegistrationCategoryType = ERegistrationCategoryType.FIRST_TIME_VENDOR,
                    },
                    new RegistrationPlan()
                    {
                        Title = "Class D",
                        Description = "Product G Registration Fee For Contract Value",
                        ContractMinValue = 2500000000,
                        ContractMaxValue = 100000000000,
                        Grade = "Class D",
                        Fee = 5000000,
                        TenureInDays = 365,
                        CreatedBy = userId,
                        RegistrationCategoryType = ERegistrationCategoryType.FIRST_TIME_VENDOR,
                    },
                    new RegistrationPlan()
                    {
                        Title = "Class E",
                        Description = "Product G Registration Fee For Contract Value",
                        ContractMinValue = 100000000000,
                        ContractMaxValue = 1000000000000000000,
                        Grade = "Class E",
                        Fee = 25000,
                        TenureInDays = 365,
                        CreatedBy = userId,
                        RegistrationCategoryType = ERegistrationCategoryType.FIRST_TIME_VENDOR,
                    }
                };

                await context.RegistrationPlans.AddRangeAsync(registrationPlans);
            }

            if (!context.ProcurementPlanTypes.Any())
            {
                var ProcurementPlanTypes = new ProcurementPlanType[]
                {
                    new ProcurementPlanType
                    {
                        Title = "Draft Bidding Document",
                        Description = "Draft Bidding Document",
                        ProcurementPlanTask = EPprocurementPlanTask.PROCUREMENTPLANNING
                    },
                    new ProcurementPlanType
                    {
                        Title = "Specific Procurement Notice",
                        Description = "Specific Procurement Notice",
                        ProcurementPlanTask = EPprocurementPlanTask.PROCUREMENTPLANNING
                    },
                    new ProcurementPlanType
                    {
                        Title = "Bid Invitation",
                        Description = "Bid Invitation",
                        ProcurementPlanTask = EPprocurementPlanTask.PROCUREMENTPLANNING
                    },
                    new ProcurementPlanType
                    {
                        Title = "Bid Submission/Opening/Minutes",
                        Description = "Bid Submission/Opening/Minutes",
                        ProcurementPlanTask = EPprocurementPlanTask.PROCUREMENTPLANNING
                    },
                    new ProcurementPlanType
                    {
                        Title = "Bid Evaluation Report And Recommendation For Awards",
                        Description = "Bid Evaluation Report And Recommendation For Awards",
                        ProcurementPlanTask = EPprocurementPlanTask.PROCUREMENTPLANNING
                    },
                    new ProcurementPlanType
                    {
                        Title = "Contract Award Decision",
                        Description = "Contract Award Decision",
                        ProcurementPlanTask = EPprocurementPlanTask.PROCUREMENTPLANNING
                    },
                    new ProcurementPlanType
                    {
                        Title = "Letter of Acceptance",
                        Description = "Letter of Acceptance",
                        ProcurementPlanTask = EPprocurementPlanTask.PROCUREMENTPLANNING
                    },
                    new ProcurementPlanType
                    {
                        Title = "Contract Signing",
                        Description = "Contract Signing",
                        ProcurementPlanTask = EPprocurementPlanTask.PROCUREMENTEXECUTION
                    },
                    new ProcurementPlanType
                    {
                        Title = "Publication of Contract Award",
                        Description = "Publication of Contract Award",
                        ProcurementPlanTask = EPprocurementPlanTask.PROCUREMENTEXECUTION
                    },
                    new ProcurementPlanType
                    {
                        Title = "Planning and Tracking",
                        Description = "Planning and Tracking",
                        ProcurementPlanTask = EPprocurementPlanTask.PROCUREMENTEXECUTION
                    },
                    new ProcurementPlanType
                    {
                        Title = "Approval of Payment",
                        Description = "Approval of Payment",
                        ProcurementPlanTask = EPprocurementPlanTask.PROCUREMENTEXECUTION
                    },
                    new ProcurementPlanType
                    {
                        Title = "Completion Report",
                        Description = "Completion Report",
                        ProcurementPlanTask = EPprocurementPlanTask.PROCUREMENTEXECUTION
                    },
                };

                context.ProcurementPlanTypes.AddRange(ProcurementPlanTypes);
            }

            if (!context.ProcurementCategories.Any())
            {
                var procurementCategories = new ProcurementCategory[]
                {
                    new ProcurementCategory
                    {
                        Name = "Goods",
                        Description = "Procurement Category - Goods",
                        CreateAt = DateTime.UtcNow
                    },

                    new ProcurementCategory
                    {
                        Name = "Works",
                        Description = "Procurement Category - Works",
                        CreateAt = DateTime.UtcNow
                    },

                    new ProcurementCategory
                    {
                        Name = "Non Consultation",
                        Description = "Procurement Category - Non Consultation",
                        CreateAt = DateTime.UtcNow
                    },

                    new ProcurementCategory
                    {
                        Name = "Consultation",
                        Description = "Procurement Category - Consultation",
                        CreateAt = DateTime.UtcNow
                    }
                };

                context.ProcurementCategories.AddRange(procurementCategories);
            }

            if (!context.ProcurementProcesses.Any())
            {
                var procurementProcesses = new ProcurementProcess[]
                {
                    new ProcurementProcess
                    {
                        Name = "Single Stage - Single Envelope",
                        Description = "Single Stage - Single Envelope",
                        CreateAt = DateTime.UtcNow
                    },

                    new ProcurementProcess
                    {
                        Name = "Single Stage - Double Envelope",
                        Description = "Single Stage - Double Envelope",
                        CreateAt = DateTime.UtcNow
                    }
                };

                context.ProcurementProcesses.AddRange(procurementProcesses);
            }

            if (!context.ReviewMethods.Any())
            {
                var reviewMethods = new ReviewMethod[]
                {
                    new ReviewMethod
                    {
                        Name = "Post Review",
                        Description = "Post Review",
                        CreateAt = DateTime.UtcNow
                    },

                    new ReviewMethod
                    {
                        Name = "Prior Review",
                        Description = "Prior Review",
                        CreateAt = DateTime.UtcNow
                    }
                };

                context.ReviewMethods.AddRange(reviewMethods);
            }

            if (!context.QualificationMethods.Any())
            {
                var qualificationMethods = new QualificationMethod[]
                {
                    new QualificationMethod
                    {
                        Name = "Pre Qualification",
                        Description = "Pre Qualification",
                        CreateAt = DateTime.UtcNow
                    },

                    new QualificationMethod
                    {
                        Name = "Post Qualification",
                        Description = "Post Qualification",
                        CreateAt = DateTime.UtcNow
                    }
                };

                context.QualificationMethods.AddRange(qualificationMethods);
            }

            if (!context.ProcurementMethods.Any())
            {
                var procurementMethods = new ProcurementMethod[]
                {
                    new ProcurementMethod
                    {
                        Name = "International Competitive Bidding (ICB)",
                        Code = "ICB",
                        Description = "International Competitive Bidding (ICB)",
                        CreateAt = DateTime.UtcNow
                    },

                    new ProcurementMethod
                    {
                        Name = "Limited International Bidding (LIB)",
                        Code = "LIB",
                        Description = "Limited International Bidding (LIB)",
                        CreateAt = DateTime.UtcNow
                    },

                    new ProcurementMethod
                    {
                        Name = "National Competitive Bidding (NCB)",
                        Code = "NCB",
                        Description = "National Competitive Bidding (NCB)",
                        CreateAt = DateTime.UtcNow
                    },

                    new ProcurementMethod
                    {
                        Name = "Direct Contracting",
                        Code = "DC",
                        Description = "Direct Contracting",
                        CreateAt = DateTime.UtcNow
                    },

                    new ProcurementMethod
                    {
                        Name = "National",
                        Code = "National",
                        Description = "National",
                        CreateAt = DateTime.UtcNow
                    },

                    new ProcurementMethod
                    {
                        Name = "Shopping",
                        Code = "Shopping",
                        Description = "Shopping",
                        CreateAt = DateTime.UtcNow
                    },

                    new ProcurementMethod
                    {
                        Name = "Force Account (Direct Labour)",
                        Description = "Force Account (Direct Labour)",
                        CreateAt = DateTime.UtcNow
                    }
                };

                context.ProcurementMethods.AddRange(procurementMethods);
            }

            if (!context.Roles.Any())
            {

                var roles = new Role[]
                {
                    new Role
                    {
                        Title = "Procurement Officer",
                        Description = "Create Procurement plans and send for approval for his/her ministry",
                        Type = "SYSTEM",
                        CreateAt = DateTime.Now
                    },
                    new Role
                    {
                        Title = "Permanent Secretary",
                        Description = "View all procurements that relates to his/her ministry and approve less than the set amount",
                        Type = "SYSTEM",
                        CreateAt = DateTime.Now
                    },
                    new Role
                    {
                        Title = "Commissioner",
                        Description = "View procurements across all the Ministries and approve bids above the set amount",
                        Type = "SYSTEM",
                        CreateAt = DateTime.Now
                    },
                    new Role
                    {
                        Title = "Executive",
                        Description = "Executive",
                        Type = "SYSTEM",
                        CreateAt = DateTime.Now
                    },
                    new Role
                    {
                        Title = "Accountant",
                        Description = "Accountant",
                        Type = "SYSTEM",
                        CreateAt = DateTime.Now
                    },
                    new Role
                    {
                        Title = "Audit/store personnel",
                        Description = "Audit/store personnel",
                        Type = "SYSTEM",
                        CreateAt = DateTime.Now
                    },
                    new Role
                    {
                        Title = "Bureau of Public Procurement",
                        Description = "Bureau of Public Procurement",
                        Type = "SYSTEM",
                        CreateAt = DateTime.Now
                    }
                };

                await context.Roles.AddRangeAsync(roles);
            }
            // save changes to the database
            await context.SaveChangesAsync();
           
        }
    }
}
