using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class VendorServicesComposite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // drop VendorServices Table
            migrationBuilder.DropTable(
                 name: "VendorServices");

            migrationBuilder.CreateTable(
                name: "VendorServices",
                columns: table => new
                {
                    //Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessServiceID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorServices", x => new { x.UserID, x.BusinessServiceID });
                });
            //migrationBuilder.DropForeignKey(
            //    name: "FK_BusinessServices_VendorServices_VendorServiceId",
            //    table: "BusinessServices");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Users_VendorServices_VendorServiceId",
            //    table: "Users");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_VendorServices_Users_UserID",
            //    table: "VendorServices");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_VendorServices",
            //    table: "VendorServices");

            //migrationBuilder.DropIndex(
            //    name: "IX_VendorServices_UserID",
            //    table: "VendorServices");

            //migrationBuilder.DropIndex(
            //    name: "IX_Users_VendorServiceId",
            //    table: "Users");

            //migrationBuilder.DropIndex(
            //    name: "IX_BusinessServices_VendorServiceId",
            //    table: "BusinessServices");

            //migrationBuilder.DropColumn(
            //    name: "Id",
            //    table: "VendorServices");

            //migrationBuilder.DropColumn(
            //    name: "VendorServiceId",
            //    table: "Users");

            //migrationBuilder.DropColumn(
            //    name: "VendorServiceId",
            //    table: "BusinessServices");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_VendorServices",
            //    table: "VendorServices",
            //    columns: new[] { "UserID", "BusinessServiceID" });

            migrationBuilder.CreateIndex(
                name: "IX_VendorServices_BusinessServiceID",
                table: "VendorServices",
                column: "BusinessServiceID");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorServices_BusinessServices_BusinessServiceID",
                table: "VendorServices",
                column: "BusinessServiceID",
                principalTable: "BusinessServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorServices_Users_UserID",
                table: "VendorServices",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorServices_BusinessServices_BusinessServiceID",
                table: "VendorServices");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorServices_Users_UserID",
                table: "VendorServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorServices",
                table: "VendorServices");

            migrationBuilder.DropIndex(
                name: "IX_VendorServices_BusinessServiceID",
                table: "VendorServices");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "VendorServices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "VendorServiceId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VendorServiceId",
                table: "BusinessServices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorServices",
                table: "VendorServices",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_VendorServices_UserID",
                table: "VendorServices",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_VendorServiceId",
                table: "Users",
                column: "VendorServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessServices_VendorServiceId",
                table: "BusinessServices",
                column: "VendorServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessServices_VendorServices_VendorServiceId",
                table: "BusinessServices",
                column: "VendorServiceId",
                principalTable: "VendorServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_VendorServices_VendorServiceId",
                table: "Users",
                column: "VendorServiceId",
                principalTable: "VendorServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorServices_Users_UserID",
                table: "VendorServices",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
