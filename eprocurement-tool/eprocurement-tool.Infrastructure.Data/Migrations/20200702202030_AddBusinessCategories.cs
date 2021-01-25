using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class AddBusinessCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_vendorDocumentTypes",
                table: "vendorDocumentTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vendorDirectors",
                table: "vendorDirectors");

            migrationBuilder.RenameTable(
                name: "vendorDocumentTypes",
                newName: "VendorDocumentTypes");

            migrationBuilder.RenameTable(
                name: "vendorDirectors",
                newName: "VendorDirectors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorDocumentTypes",
                table: "VendorDocumentTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorDirectors",
                table: "VendorDirectors",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BusinessCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatedByID = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessCategories_Users_CreatedByID",
                        column: x => x.CreatedByID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCategories_CreatedByID",
                table: "BusinessCategories",
                column: "CreatedByID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorDocumentTypes",
                table: "VendorDocumentTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorDirectors",
                table: "VendorDirectors");

            migrationBuilder.RenameTable(
                name: "VendorDocumentTypes",
                newName: "vendorDocumentTypes");

            migrationBuilder.RenameTable(
                name: "VendorDirectors",
                newName: "vendorDirectors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vendorDocumentTypes",
                table: "vendorDocumentTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vendorDirectors",
                table: "vendorDirectors",
                column: "Id");
        }
    }
}
