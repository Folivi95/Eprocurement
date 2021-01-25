using AutoMapper;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Application.Repository;
using EGPS.Application.Services;
using EGPS.Application.Validators;
using eprocurement_tool.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace EGPS.Infrastructure.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMvc().AddFluentValidation();
            services.AddTransient<IValidator<AccountForCreationDTO>, AccountForCreationDtoValidator>();
            services.AddTransient<IValidator<UserForCreationDTO>, UserForCreationDtoValidator>();
            services.AddTransient<IValidator<UserVendorForCreationDTO>, UserVendorForCreationDTOValidator>();
            services.AddTransient<IValidator<RoleForCreationDTO>, RoleForCreationDtoValidator>();
            services.AddTransient<IValidator<DocumentClassForCreationDTO>, DocumentClassForCreationDtoValidator>();
            services.AddTransient<IValidator<UserInvitationForCreationDTO>, UserInvitationForCreationDtoValidator>();
            services.AddTransient<IValidator<UserActivityForCreationDTO>, UserActivityForCreationDtoValidator>();
            services.AddTransient<IValidator<NotificationForCreationDTO>, NotificationForCreationDtoValidator>();
            services.AddTransient<IValidator<DepartmentForCreationDTO>, DepartmentForCreationDtoValidator>();
            services.AddTransient<IValidator<UnitForCreationDTO>, UnitForCreationDtoValidator>();
            services.AddTransient<IValidator<WorkflowForCreationDTO>, WorkflowForCreationValidator>();
            services.AddTransient<IValidator<StageForCreationDTO>, StageForCreationDtoValidator>();
            services.AddTransient<IValidator<PasswordResetForCreationDTO>, PasswordResetForCreationDtoValidator>();
            services.AddTransient<IValidator<ResendAccountMailDTO>, ResendAccountMailDtoValidator>();
            services.AddTransient<IValidator<ResendInvitationMailDTO>, ResendInvitationMailDtoValidator>();
            services.AddTransient<IValidator<UserLoginForCreationDTO>, UserLoginForCreationDtoValidator>();
            services.AddTransient<IValidator<UserChangePasswordForCreationDTO>, UserChangePasswordForCreationDtoValidator>();
            services.AddTransient<IValidator<PasswordResetLinkForCreationDTO>, PasswordResetLinkForCreationDtoValidator>();
            services.AddTransient<IValidator<UserForUpdateDTO>, UserForUpdateDtoValidator>();
            services.AddTransient<IValidator<AccountForUpdateDTO>, AccountForUpdateValidator>();
            services.AddTransient<IValidator<DocumentClassForUpdateDTO>, DocumentClassForUpdateDtoValidator>();
            services.AddTransient<IValidator<UnitForUpdateDTO>, UnitForUpdateDtoValidator>();
            services.AddTransient<IValidator<DepartmentForUpdateDTO>, DepartmentForUpdateDtoValidator>();
            services.AddTransient<IValidator<StageForUpdateDTO>, StageForUpdateDtoValidator>();
            services.AddTransient<IValidator<StageForUnderWorkflowUpdateDTO>, StageForUpdateDtoUnderWorkflowValidator>();
            services.AddTransient<IValidator<WorkflowForUpdateDTO>, WorkflowForUpdateValidator>();
            services.AddTransient<IValidator<UsersMultipleInvitesForCreationDTO>, UsersMultipleInvitesForCreationDtoValidator>();
            services.AddTransient<IValidator<VendorProfileForCreationDTO>, VendorProfileForCreationDtoValidator>();
            services.AddTransient<IValidator<VendorProfileForUpdateDTO>, VendorProfileForUpdateDtoValidator>();
            services.AddTransient<IValidator<VendorDirectorForCreationDTO>, VendorDirectorForCreationDtoValidator>();
            services.AddTransient<IValidator<VendorDirectorForUpdateDTO>, VendorDirectorForUpdateDtoValidator>();
            services.AddTransient<IValidator<VendorDocumentForCreationDTO>, VendorDocumentForCreationDtoValidator>();
            services.AddTransient<IValidator<RegistrationPlanForCreationDTO>, RegistrationPlanForCreationDtoValidator>();
            services.AddTransient<IValidator<StaffForUpdateDTO>, StaffForUpdateDtoValidator>();
            services.AddTransient<IValidator<ProcurementPlanForCreationDTO>, ProcurementPlanForCreationDtoValidator>();
            services.AddTransient<IValidator<MinistryForCreationDTO>, MinistryForCreationDtoValidator>();
            services.AddTransient<IValidator<CommentForCreation>, CommentForCreationDtoValidator>();
            services.AddTransient<IValidator<ReviewForCreation>, ReviewForCreationValidator>();
            services.AddTransient<IValidator<NoticeInformationForCreation>, NoticeInformationForCreationValidator>();
            services.AddTransient<IValidator<ContractAwardDocumentCreation>, ContractAwardDocumentForCreationValidator>();
            services.AddTransient<IValidator<DocumentDatasheetCreation>, DocumentDatasheetForCreationValidator>();
            services.AddTransient<IValidator<ContractSigningDocumentAndDatasheetCreation>, ContractSigningDocumentForCreationValidator>();
            services.AddTransient<IValidator<BidEvaluationForCreation>, BidEvaluationValidator>();
            services.AddTransient<IValidator<ProcurementActivityForCreation>, ActivityCreationValidator>();
            services.AddTransient<IValidator<GeneralPlanForCreation>, GeneralPlanForCreationValidator>();
            services.AddTransient<IValidator<GeneralPlanForUpdate>, GeneralPlanForUpdateValidator>();
            services.AddTransient<IValidator<ProcurementPlanForUpdateDTO>, ProcurementPlanForUpdateDtoValidator>();
            services.AddTransient<IValidator<MilestoneTaskForCreateDTO>, MilestoneTaskForCreateDTOValidator>();
            services.AddTransient<IValidator<MilestoneInvoiceForCreation>, MilestoneInvoiceForCreationValidator>();
            services.AddTransient<IValidator<DocumentUploadDto>, DocumentUploadValidator>();
            services.AddTransient<IHostedService, ContractStatusService>();


            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserActivityRepository, UserActivityRepository>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IEmailLogger, EmailLogger>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserInvitationRepository, UserInvitationRepository>();
            services.AddScoped<IDocumentClassRepository, DocumentClassRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IWorkflowRepository, WorkflowRepository>();
            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IVendorProfileRepository, VendorProfileRepository>();
            services.AddScoped<IVendorContactRepository, VendorContactRepository>();
            services.AddScoped<IVendorCorrespondenceRepository, VendorCorrespondenceRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<IVendorAttestationRepository, VendorAttestationRepository>();
            services.AddScoped<IVendorDirectorRepository, VendorDirectorRepository>();
            services.AddScoped<IVendorDirectorCertificateRepository, VendorDirectorCertificateRepository>();
            services.AddScoped<IVendorDocumentRepository, VendorDocumentRepository>();
            services.AddScoped<IVendorDocumentTypeRepository, VendorDocumentTypeRepository>();
            services.AddScoped<IBusinessServiceRepository, BusinessServiceRepository>();
            services.AddScoped<IBusinessCategoryRepository, BusinessCategoryRepository>();
            services.AddScoped<IVendorDocumentTypeRepository, VendorDocumentTypeRepository>();
            services.AddScoped<IVendorServiceRepository, VendorServiceRepository>();
            services.AddScoped<IRegistrationPlanRepository, RegistrationPlanRepository>();
            services.AddScoped<IVendorRegistrationCategoryRepository, VendorRegistrationCategoryRepository>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddScoped<IMinistryRepository, MinistryRepository>();
            services.AddScoped<IProcurementPlanRepository, ProcurementPlanRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<IGeneralPlanRepository, GeneralPlanRepository>();
            services.AddScoped<IBidRepository, BidRepository>();
            services.AddScoped<IVendorProcurementRepository, VendorProcurementRepository>();
            services.AddScoped<IVendorBidRepository, VendorBidRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IProjectMileStoneRepository, ProjectMileStoneRepository>();
            services.AddScoped<IMileStoneTaskRepository, MileStoneTaskRepository>();
            services.AddScoped<IMilestoneInvoiceRepository, MilestoneInvoiceRepository>();
            services.AddScoped<IEmailTemplate, EmailTemplates>();
            services.AddScoped<IProcurementPlanActivityRepository, ProcurementPlanActivityRepository>();

            services.AddSingleton<IJwtAuthenticationManager, JwtAuthenticateManager>();
            services.AddScoped<IPhotoAcessor, PhotoAcessor>();
            services.AddScoped<IProcurementService, ProcurementService>();
            services.AddScoped<IDocumentUploadRepository, DocumentUploadRepository>();
            services.AddScoped<IDocumentUploadService, DocumentUploadService>();

            services.AddMvc().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }
    }
}
