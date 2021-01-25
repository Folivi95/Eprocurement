using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Migrations;
using Microsoft.EntityFrameworkCore;

namespace EGPS.Infrastructure.Data.Context
{
    public class EDMSDBContext : DbContext
    {
        public EDMSDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<DocumentClass>()
                   .HasOne<User>().WithMany().HasForeignKey(d => d.CreatedById)
                   .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<DocumentClass>()
                   .HasOne<Account>().WithMany().HasForeignKey(d => d.AccountId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserInvitation>()
               .HasIndex(u => u.Email)
               .IsUnique();

            builder.Entity<UserInvitation>()
               .HasOne(x => x.Account)
               .WithMany()
               .HasForeignKey(u => u.AccountId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserActivity>()
                .HasOne(u => u.User)
                .WithMany(u => u.UserActivities)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Department>()
                   .HasOne<User>().WithMany().HasForeignKey(d => d.CreatedById)
                   .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Department>()
                   .HasOne<Account>().WithMany().HasForeignKey(d => d.AccountId)
                   .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Department>()
                   .HasOne<User>().WithMany().HasForeignKey(d => d.LeadId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Notification>()
                .Property(c => c.Type)
                .HasConversion<int>();

            builder.Entity<Notification>()
                .Property(c => c.NotificationType)
                .HasConversion<int>();

            //builder.Entity<Notification>()
            //    .HasOne<User>()
            //    .WithMany()
            //    .HasForeignKey(u => u.UserId)
            //    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Unit>()
                   .HasOne<User>().WithMany().HasForeignKey(d => d.CreatedById)
                   .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Unit>()
                   .HasOne<User>().WithMany().HasForeignKey(d => d.LeadId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Stage>()
               .HasOne<Account>()
               .WithMany()
               .HasForeignKey(u => u.AccountId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PasswordReset>()
                   .HasOne<Account>()
                   .WithMany()
                   .HasForeignKey(u => u.AccountId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<DepartmentMember>()
                   .HasOne(d => d.Department)
                   .WithMany(d => d.DepartmentMembers)
                   .HasForeignKey(d => d.DepartmentId);

            builder.Entity<DepartmentMember>()
                   .HasOne(d => d.User)
                   .WithMany(d => d.DepartmentMembers)
                   .HasForeignKey(d => d.UserId);

            builder.Entity<UnitMember>()
                   .HasOne(u => u.Unit)
                   .WithMany(u => u.UnitMembers)
                   .HasForeignKey(d => d.UnitId);

            builder.Entity<UnitMember>()
                   .HasOne(u => u.User)
                   .WithMany(u => u.UnitMembers)
                   .HasForeignKey(d => d.UserId);

            builder.Entity<Role>()
                   .HasOne<User>()
                   .WithMany()
                   .HasForeignKey(d => d.CreatedById)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Role>()
                   .HasOne<Account>()
                   .WithMany()
                   .HasForeignKey(d => d.AccountId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RoleResource>()
                .HasIndex(b => b.ResourceId)
                .IsUnique(false);

            builder.Entity<Workflow>()
                   .HasOne<User>()
                   .WithMany()
                   .HasForeignKey(u => u.CreatedById)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            builder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);
            builder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            builder.Entity<Comment>()
                   .HasOne(ur => ur.CreatedBy)
                   .WithMany()
                   .HasForeignKey(ur => ur.CreatedById)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Comment>()
                   .HasOne(ur => ur.Parent)
                   .WithMany(r => r.comments)
                   .HasForeignKey(ur => ur.ParentId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Review>()
                .HasOne(ur => ur.CreatedBy)
                .WithMany()
                .HasForeignKey(ur => ur.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ProcurementPlanActivity>()
                .HasOne(pr => pr.ProcurementPlan)
                .WithMany(p => p.ProcurementPlanActivities)
                .HasForeignKey(u => u.ProcurementPlanId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ProcurementPlanActivity>()
                .HasOne(pr => pr.CreatedBy)
                .WithMany()
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ProcurementPlanDocument>()
                .HasOne(pr => pr.CreatedBy)
                .WithMany()
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<NoticeInformation>()
                .HasOne(pr => pr.CreatedBy)
                .WithMany()
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<NoticeInformation>()
                .HasOne(pr => pr.ProcurementPlan)
                .WithMany()
                .HasForeignKey(u => u.ProcurementPlanId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Datasheet>()
                .HasOne(pr => pr.ProcurementPlanActivity)
                .WithMany()
                .HasForeignKey(u => u.ProcurementPlanActivityId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Datasheet>()
                .HasOne(pr => pr.CreatedBy)
                .WithMany()
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<VendorProcurement>().HasKey(ma => new { ma.VendorId, ma.ProcurementPlanId });

            builder.Entity<VendorProcurement>()
                .HasOne(a => a.ProcurementPlan)
                .WithMany(a => a.VendorProcurements)
                .HasForeignKey(sc => sc.ProcurementPlanId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<VendorProcurement>()
                .HasOne(a => a.Vendor)
                .WithMany(a => a.VendorProcurements)
                .HasForeignKey(sc => sc.VendorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Contract>()
                .HasOne(a => a.Contractor)
                .WithMany(a => a.Contracts)
                .HasForeignKey(sc => sc.ContractorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<GeneralPlan>()
               .HasOne(a => a.Ministry)
               .WithMany()
               .HasForeignKey(sc => sc.MinistryId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<GeneralPlan>()
                .HasOne(a => a.CreatedBy)
                .WithMany()
                .HasForeignKey(sc => sc.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ProcurementPlan>()
                .HasOne(a => a.GeneralPlan)
                .WithMany(a => a.ProcurementPlans)
                .HasForeignKey(sc => sc.GeneralPlanId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<ProcurementPlan>()
                .HasOne(a => a.ProcurementCategory)
                .WithMany()
                .HasForeignKey(sc => sc.ProcurementCategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ProcurementPlan>()
                .HasOne(a => a.Ministry)
                .WithMany(m => m.ProcurementPlans)
                .HasForeignKey(sc => sc.MinistryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ProcurementPlan>()
                .HasOne(a => a.ProcurementMethod)
                .WithMany()
                .HasForeignKey(sc => sc.ProcurementMethodId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ProcurementPlan>()
                .HasOne(a => a.ProcurementProcess)
                .WithMany()
                .HasForeignKey(sc => sc.ProcessTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ProcurementPlan>()
                .HasOne(a => a.ProcurementMethod)
                .WithMany()
                .HasForeignKey(sc => sc.ProcurementMethodId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ProcurementPlan>()
                .HasOne(a => a.QualificationMethod)
                .WithMany()
                .HasForeignKey(sc => sc.QualificationMethodId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ProcurementPlan>()
                .HasOne(a => a.ReviewMethod)
                .WithMany()
                .HasForeignKey(sc => sc.ReviewMethodId)
                .OnDelete(DeleteBehavior.NoAction);

            //builder.Entity<Project>()
            //       .HasOne(a => a.Contract)
            //       .WithMany()
            //       .HasForeignKey(sc => sc.ContractId)
            //       .OnDelete(DeleteBehavior.NoAction);

            //builder.Entity<ProjectMileStone>()
            //       .HasOne(a => a.Project)
            //       .WithMany(x => x.ProjectMileStones)
            //       .HasForeignKey(sc => sc.ProjectId)
            //       .OnDelete(DeleteBehavior.NoAction);

            //builder.Entity<ProjectMileStone>()
            //       .HasOne(a => a.CreatedBy)
            //       .WithMany()
            //       .HasForeignKey(sc => sc.CreatedById)
            //       .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MilestoneTask>()
                  .HasOne(a => a.MileStone)
                  .WithMany(x => x.MilestoneTasks)
                  .HasForeignKey(x => x.MileStoneId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProjectMileStone>()
                .HasOne(a => a.MilestoneInvoice)
                .WithOne(b => b.ProjectMileStone)
                .HasForeignKey<MilestoneInvoice>(d => d.ProjectMileStoneId);

            builder.Entity<Ministry>()
                .HasMany(a => a.Projects)
                .WithOne(b => b.Ministry);

            builder.Entity<ProcurementPlan>()
                   .HasOne(a => a.CreatedBy)
                   .WithMany()
                   .HasForeignKey(x => x.CreatedById)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Document>()
                   .HasOne(a => a.CreatedBy)
                   .WithMany()
                   .HasForeignKey(x => x.CreatedById)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<DocumentClass>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<Role>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<Stage>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<Unit>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<Department>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<UserInvitation>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<Workflow>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<VendorService>().HasKey(x => new { x.UserID, x.BusinessServiceID });
            builder.Entity<VendorRegistrationCategory>().HasKey(x => new { x.UserId, x.RegistrationPlanId });
            builder.Entity<Comment>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<Bid>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<MilestoneTask>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<ProjectMileStone>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<Project>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<MilestoneInvoice>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<ProcurementPlanDocument>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<ProjectMileStone>().HasQueryFilter(x => !x.Deleted);
            builder.Entity<Document>().HasQueryFilter(x => !x.Deleted);
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<DocumentClass> DocumentClasses { get; set; }
        public DbSet<UserInvitation> UserInvitations { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }
        public DbSet<DepartmentMember> DepartmentMembers { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<RoleResource> RoleResources { get; set; }
        public DbSet<UnitMember> UnitMembers { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<VendorProfile> VendorProfiles { get; set; }
        public DbSet<VendorDocumentType> VendorDocumentTypes { get; set; }
        public DbSet<VendorDirector> VendorDirectors { get; set; }
        public DbSet<VendorDirectorCertificate> VendorDirectorCertificates { get; set; }
        public DbSet<BusinessCategory> BusinessCategories { get; set; }
        public DbSet<BusinessService> BusinessServices { get; set; }
        public DbSet<VendorCorrespondence> VendorCorrespondences { get; set; }
        public DbSet<VendorAttestation> VendorAttestations { get; set; }
        public DbSet<VendorDocument> VendorDocuments { get; set; }
        public DbSet<VendorService> VendorServices { get; set; }
        public DbSet<Ministry> Ministries { get; set; }
        public DbSet<StaffProfile> StaffProfiles { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<BidType> BidTypes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<VendorContact> vendorContacts { get; set; }
        public DbSet<RegistrationPlan> RegistrationPlans { get; set; }
        public DbSet<VendorRegistrationCategory> VendorRegistrationCategories { get; set; }
        public DbSet<ProcurementPlan> ProcurementPlans { get; set; }
        public DbSet<ProcurementDocumentType> ProcurementDocumentTypes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ProcurementPlanType> ProcurementPlanTypes { get; set; }
        public DbSet<ProcurementCategory> ProcurementCategories { get; set; }
        public DbSet<ProcurementMethod> ProcurementMethods { get; set; }
        public DbSet<ReviewMethod> ReviewMethods { get; set; }
        public DbSet<QualificationMethod> QualificationMethods { get; set; }
        public DbSet<ProcurementProcess> ProcurementProcesses { get; set; }
        public DbSet<ProcurementPlanActivity> ProcurementPlanActivities { get; set; }
        public DbSet<ProcurementPlanDocument> ProcurementPlanDocuments { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<NoticeInformation> NoticeInformations { get; set; }
        public DbSet<Datasheet> Datasheets { get; set; }
        public DbSet<VendorProcurement> VendorProcurements { get; set; }
        public DbSet<ProcurementPlanNumber> ProcurementPlanNumbers { get; set; }
        public DbSet<GeneralPlan> GeneralPlans { get; set; }
        public DbSet<VendorBid> VendorBids { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMileStone> ProjectMileStones { get; set; }
        public DbSet<MilestoneTask> MilestoneTasks { get; set; }
        public DbSet<MilestoneInvoice> MilestoneInvoices { get; set; }
        public DbSet<Document> Documents { get; set; }
    }

}
