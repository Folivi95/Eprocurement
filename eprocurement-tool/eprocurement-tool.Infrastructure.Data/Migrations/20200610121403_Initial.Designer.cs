﻿// <auto-generated />
using System;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EGPS.Infrastructure.Data.Migrations
{
    [DbContext(typeof(EDMSDBContext))]
    [Migration("20200610121403_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EDMS.Domain.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AddressLine1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddressLine2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BusinessType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyLogo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactPhone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TimeZone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Website")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("EDMS.Domain.Entities.Action", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ActionType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Action");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ActionType = "Checker"
                        },
                        new
                        {
                            Id = 2,
                            ActionType = "Approver"
                        });
                });

            modelBuilder.Entity("EDMS.Domain.Entities.Department", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("LeadId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Website")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LeadId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("EDMS.Domain.Entities.DepartmentMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("UserId");

                    b.ToTable("DepartmentMembers");
                });

            modelBuilder.Entity("EDMS.Domain.Entities.DocumentClass", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("DocumentClasses");
                });

            modelBuilder.Entity("EDMS.Domain.Entities.GroupClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("GroupClassType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GroupClass");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            GroupClassType = "All"
                        },
                        new
                        {
                            Id = 2,
                            GroupClassType = "Department"
                        },
                        new
                        {
                            Id = 3,
                            GroupClassType = "Unit"
                        });
                });

            modelBuilder.Entity("EDMS.Domain.Entities.NType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Types");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Type = "Email"
                        },
                        new
                        {
                            Id = 2,
                            Type = "SMS"
                        },
                        new
                        {
                            Id = 3,
                            Type = "Push"
                        },
                        new
                        {
                            Id = 4,
                            Type = "InApp"
                        });
                });

            modelBuilder.Entity("EDMS.Domain.Entities.NotificaitonType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Notification")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("NotificaitonTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Notification = "Email"
                        },
                        new
                        {
                            Id = 2,
                            Notification = "Feed"
                        });
                });

            modelBuilder.Entity("EDMS.Domain.Entities.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Body")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<int>("NotificationType")
                        .HasColumnType("int");

                    b.Property<string>("Recipient")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TemplateId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("EDMS.Domain.Entities.PasswordReset", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("PasswordResets");
                });

            modelBuilder.Entity("EDMS.Domain.Entities.Resource", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("EDMS.Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("CreatedById");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("EDMS.Domain.Entities.RoleResource", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Permissions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ResourceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ResourceId");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleResources");
                });

            modelBuilder.Entity("EDMS.Domain.Entities.Stage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Action")
                        .HasColumnType("int");

                    b.Property<string>("AssigneeIds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("GroupClass")
                        .HasColumnType("int");

                    b.Property<string>("GroupIds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MinimumPass")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.Property<Guid>("WorkFlowId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("WorkFlowId");

                    b.ToTable("Stages");
                });

            modelBuilder.Entity("EDMS.Domain.Entities.Unit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("LeadId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Website")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("LeadId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Units");
                });

            modelBuilder.Entity("EDMS.Domain.Entities.UnitMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UnitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UnitId");

                    b.HasIndex("UserId");

                    b.ToTable("UnitMembers");
                });

            modelBuilder.Entity("EDMS.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("EmailVerified")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Lastlogin")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UnitId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("VerificationToken")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.HasIndex("UnitId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EDMS.Domain.Entities.UserActivity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IpAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ObjectClass")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ObjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserActivities");
                });

            modelBuilder.Entity("EDMS.Domain.Entities.UserInvitation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.ToTable("UserInvitations");
                });

            modelBuilder.Entity("EDMS.Domain.Entities.Workflow", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Workflows");
                });

            modelBuilder.Entity("EDMS.Domain.Entities.Department", b =>
                {
                    b.HasOne("EDMS.Domain.Entities.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EDMS.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EDMS.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("LeadId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("EDMS.Domain.Entities.DepartmentMember", b =>
                {
                    b.HasOne("EDMS.Domain.Entities.Department", null)
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EDMS.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("EDMS.Domain.Entities.DocumentClass", b =>
                {
                    b.HasOne("EDMS.Domain.Entities.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EDMS.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("EDMS.Domain.Entities.Notification", b =>
                {
                    b.HasOne("EDMS.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("EDMS.Domain.Entities.PasswordReset", b =>
                {
                    b.HasOne("EDMS.Domain.Entities.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("EDMS.Domain.Entities.Role", b =>
                {
                    b.HasOne("EDMS.Domain.Entities.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EDMS.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("EDMS.Domain.Entities.RoleResource", b =>
                {
                    b.HasOne("EDMS.Domain.Entities.Resource", "Resource")
                        .WithMany()
                        .HasForeignKey("ResourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EDMS.Domain.Entities.Role", null)
                        .WithMany("Resources")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EDMS.Domain.Entities.Stage", b =>
                {
                    b.HasOne("EDMS.Domain.Entities.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EDMS.Domain.Entities.Workflow", null)
                        .WithMany()
                        .HasForeignKey("WorkFlowId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("EDMS.Domain.Entities.Unit", b =>
                {
                    b.HasOne("EDMS.Domain.Entities.Account", "Account")
                        .WithMany("Units")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EDMS.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EDMS.Domain.Entities.Department", "Department")
                        .WithMany("Units")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EDMS.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("LeadId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("EDMS.Domain.Entities.UnitMember", b =>
                {
                    b.HasOne("EDMS.Domain.Entities.Unit", null)
                        .WithMany()
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EDMS.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("EDMS.Domain.Entities.User", b =>
                {
                    b.HasOne("EDMS.Domain.Entities.Department", "Department")
                        .WithMany("Users")
                        .HasForeignKey("DepartmentId");

                    b.HasOne("EDMS.Domain.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("EDMS.Domain.Entities.Unit", "Unit")
                        .WithMany("Users")
                        .HasForeignKey("UnitId");
                });

            modelBuilder.Entity("EDMS.Domain.Entities.UserActivity", b =>
                {
                    b.HasOne("EDMS.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("EDMS.Domain.Entities.UserInvitation", b =>
                {
                    b.HasOne("EDMS.Domain.Entities.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EDMS.Domain.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("EDMS.Domain.Entities.Workflow", b =>
                {
                    b.HasOne("EDMS.Domain.Entities.Account", "Account")
                        .WithMany("Workflows")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
