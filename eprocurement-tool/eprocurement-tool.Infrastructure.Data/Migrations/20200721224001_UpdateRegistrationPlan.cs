using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class UpdateRegistrationPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistrationPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Grade = table.Column<string>(nullable: true),
                    Fee = table.Column<decimal>(nullable: false),
                    ContractMinValue = table.Column<decimal>(nullable: false),
                    ContractMaxValue = table.Column<decimal>(nullable: false),
                    RegistrationCategoryType = table.Column<int>(nullable: false),
                    TenureInDays = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationPlans", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendorDocuments_VendorDocumentTypeId",
                table: "VendorDocuments",
                column: "VendorDocumentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorDocuments_VendorDocumentTypes_VendorDocumentTypeId",
                table: "VendorDocuments",
                column: "VendorDocumentTypeId",
                principalTable: "VendorDocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorDocuments_VendorDocumentTypes_VendorDocumentTypeId",
                table: "VendorDocuments");

            migrationBuilder.DropTable(
                name: "RegistrationPlans");

            migrationBuilder.DropIndex(
                name: "IX_VendorDocuments_VendorDocumentTypeId",
                table: "VendorDocuments");
        }
    }
}
