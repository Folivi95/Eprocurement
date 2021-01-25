using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class addGeneralPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GeneralPlanId",
                table: "ProcurementPlans",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ProcurementPlans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GeneralPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Fax = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    MinistryId = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralPlans_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GeneralPlans_Ministries_MinistryId",
                        column: x => x.MinistryId,
                        principalTable: "Ministries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementPlans_GeneralPlanId",
                table: "ProcurementPlans",
                column: "GeneralPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementPlans_MinistryId",
                table: "ProcurementPlans",
                column: "MinistryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementPlans_ProcessTypeId",
                table: "ProcurementPlans",
                column: "ProcessTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementPlans_ProcurementCategoryId",
                table: "ProcurementPlans",
                column: "ProcurementCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementPlans_ProcurementMethodId",
                table: "ProcurementPlans",
                column: "ProcurementMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementPlans_QualificationMethodId",
                table: "ProcurementPlans",
                column: "QualificationMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementPlans_ReviewMethodId",
                table: "ProcurementPlans",
                column: "ReviewMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralPlans_CreatedById",
                table: "GeneralPlans",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralPlans_MinistryId",
                table: "GeneralPlans",
                column: "MinistryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcurementPlans_GeneralPlans_GeneralPlanId",
                table: "ProcurementPlans",
                column: "GeneralPlanId",
                principalTable: "GeneralPlans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcurementPlans_Ministries_MinistryId",
                table: "ProcurementPlans",
                column: "MinistryId",
                principalTable: "Ministries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcurementPlans_ProcurementProcesses_ProcessTypeId",
                table: "ProcurementPlans",
                column: "ProcessTypeId",
                principalTable: "ProcurementProcesses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcurementPlans_ProcurementCategories_ProcurementCategoryId",
                table: "ProcurementPlans",
                column: "ProcurementCategoryId",
                principalTable: "ProcurementCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcurementPlans_ProcurementMethods_ProcurementMethodId",
                table: "ProcurementPlans",
                column: "ProcurementMethodId",
                principalTable: "ProcurementMethods",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcurementPlans_QualificationMethods_QualificationMethodId",
                table: "ProcurementPlans",
                column: "QualificationMethodId",
                principalTable: "QualificationMethods",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcurementPlans_ReviewMethods_ReviewMethodId",
                table: "ProcurementPlans",
                column: "ReviewMethodId",
                principalTable: "ReviewMethods",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcurementPlans_GeneralPlans_GeneralPlanId",
                table: "ProcurementPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcurementPlans_Ministries_MinistryId",
                table: "ProcurementPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcurementPlans_ProcurementProcesses_ProcessTypeId",
                table: "ProcurementPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcurementPlans_ProcurementCategories_ProcurementCategoryId",
                table: "ProcurementPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcurementPlans_ProcurementMethods_ProcurementMethodId",
                table: "ProcurementPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcurementPlans_QualificationMethods_QualificationMethodId",
                table: "ProcurementPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcurementPlans_ReviewMethods_ReviewMethodId",
                table: "ProcurementPlans");

            migrationBuilder.DropTable(
                name: "GeneralPlans");

            migrationBuilder.DropIndex(
                name: "IX_ProcurementPlans_GeneralPlanId",
                table: "ProcurementPlans");

            migrationBuilder.DropIndex(
                name: "IX_ProcurementPlans_MinistryId",
                table: "ProcurementPlans");

            migrationBuilder.DropIndex(
                name: "IX_ProcurementPlans_ProcessTypeId",
                table: "ProcurementPlans");

            migrationBuilder.DropIndex(
                name: "IX_ProcurementPlans_ProcurementCategoryId",
                table: "ProcurementPlans");

            migrationBuilder.DropIndex(
                name: "IX_ProcurementPlans_ProcurementMethodId",
                table: "ProcurementPlans");

            migrationBuilder.DropIndex(
                name: "IX_ProcurementPlans_QualificationMethodId",
                table: "ProcurementPlans");

            migrationBuilder.DropIndex(
                name: "IX_ProcurementPlans_ReviewMethodId",
                table: "ProcurementPlans");

            migrationBuilder.DropColumn(
                name: "GeneralPlanId",
                table: "ProcurementPlans");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProcurementPlans");
        }
    }
}
