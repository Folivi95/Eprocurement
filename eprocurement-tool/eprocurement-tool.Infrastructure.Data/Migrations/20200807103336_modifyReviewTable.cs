using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class modifyReviewTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Review");

            migrationBuilder.AddColumn<Guid>(
                name: "ProcurementPlanDocumentId",
                table: "Review",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Review_ProcurementPlanDocumentId",
                table: "Review",
                column: "ProcurementPlanDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_ProcurementPlanDocuments_ProcurementPlanDocumentId",
                table: "Review",
                column: "ProcurementPlanDocumentId",
                principalTable: "ProcurementPlanDocuments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_ProcurementPlanDocuments_ProcurementPlanDocumentId",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Review_ProcurementPlanDocumentId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "ProcurementPlanDocumentId",
                table: "Review");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Review",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentId",
                table: "Review",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Review",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Review",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Review",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
