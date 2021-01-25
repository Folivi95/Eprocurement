using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class VendorServiceUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_VendorServices_UserID",
                table: "VendorServices",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorServices_Users_UserID",
                table: "VendorServices",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorServices_Users_UserID",
                table: "VendorServices");

            migrationBuilder.DropIndex(
                name: "IX_VendorServices_UserID",
                table: "VendorServices");
        }
    }
}
