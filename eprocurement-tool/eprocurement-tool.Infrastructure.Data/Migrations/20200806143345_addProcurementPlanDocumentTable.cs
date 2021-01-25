using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class addProcurementPlanDocumentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcurementPlanDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    File = table.Column<string>(nullable: true),
                    ProcurementDocumentStatus = table.Column<int>(nullable: false),
                    ProcurementPlanActivityId = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcurementPlanDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcurementPlanDocuments_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProcurementPlanDocuments_ProcurementPlanActivities_ProcurementPlanActivityId",
                        column: x => x.ProcurementPlanActivityId,
                        principalTable: "ProcurementPlanActivities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementPlanDocuments_CreatedById",
                table: "ProcurementPlanDocuments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementPlanDocuments_ProcurementPlanActivityId",
                table: "ProcurementPlanDocuments",
                column: "ProcurementPlanActivityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcurementPlanDocuments");
        }
    }
}
