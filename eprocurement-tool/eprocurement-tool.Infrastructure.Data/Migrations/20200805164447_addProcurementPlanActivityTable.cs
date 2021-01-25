using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class addProcurementPlanActivityTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "ProcurementPlanActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ProcurementPlanType = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    ProcurementPlanActivityStatus = table.Column<int>(nullable: false),
                    ProcurementPlanId = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcurementPlanActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcurementPlanActivities_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProcurementPlanActivities_ProcurementPlans_ProcurementPlanId",
                        column: x => x.ProcurementPlanId,
                        principalTable: "ProcurementPlans",
                        principalColumn: "Id");
                });


            migrationBuilder.CreateIndex(
                name: "IX_ProcurementPlanActivities_CreatedById",
                table: "ProcurementPlanActivities",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementPlanActivities_ProcurementPlanId",
                table: "ProcurementPlanActivities",
                column: "ProcurementPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcurementPlanActivities");
        }
    }
}
