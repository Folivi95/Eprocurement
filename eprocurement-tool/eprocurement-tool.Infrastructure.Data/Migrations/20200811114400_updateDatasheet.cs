using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class updateDatasheet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
               name: "Datasheets");

            migrationBuilder.CreateTable(
               name: "Datasheets",
               columns: table => new
               {

                   Id = table.Column<Guid>(nullable: false),
                   CreateAt = table.Column<DateTime>(nullable: false),
                   UpdatedAt = table.Column<DateTime>(nullable: true),
                   SubmissionDeadline = table.Column<DateTime>(nullable: true),
                   SignatureDate = table.Column<DateTime>(nullable: true),
                   StartDate = table.Column<DateTime>(nullable: true),
                   Description = table.Column<string>(nullable: true),
                   Reference = table.Column<string>(nullable: true),
                   ProcurementPlanActivityId = table.Column<Guid>(nullable: false),
                   CreatedById = table.Column<Guid>(nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Datasheets", x => x.Id);
                   table.ForeignKey(
                       name: "FK_Datasheets_Users_CreatedById",
                       column: x => x.CreatedById,
                       principalTable: "Users",
                       principalColumn: "Id");
                   table.ForeignKey(
                       name: "FK_Datasheets_ProcurementPlanActivities_ProcurementPlanActivityId",
                       column: x => x.ProcurementPlanActivityId,
                       principalTable: "ProcurementPlanActivities",
                       principalColumn: "Id");
               });

            migrationBuilder.CreateIndex(
                name: "IX_Datasheets_CreatedById",
                table: "Datasheets",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Datasheets_ProcurementPlanActivityId",
                table: "Datasheets",
                column: "ProcurementPlanActivityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
