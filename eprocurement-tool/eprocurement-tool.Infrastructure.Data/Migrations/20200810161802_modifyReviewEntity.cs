using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class modifyReviewEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ProcurementPlanDocuments_ProcurementPlanDocumentId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ProcurementPlanDocumentId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ProcurementPlanDocumentId",
                table: "Reviews");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProcurementPlanDocumentId",
                table: "Reviews",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProcurementPlanDocumentId",
                table: "Reviews",
                column: "ProcurementPlanDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ProcurementPlanDocuments_ProcurementPlanDocumentId",
                table: "Reviews",
                column: "ProcurementPlanDocumentId",
                principalTable: "ProcurementPlanDocuments",
                principalColumn: "Id");
        }
    }
}
